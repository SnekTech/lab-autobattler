using System;
using Godot;
using GodotUtilities;
using LabAutobattler.Data.Player;
using LabAutobattler.Data.Units;

namespace LabAutobattler.UI.Shop;

[Scene]
public partial class UnitCard : Button
{
    public event Action<UnitStats>? UnitBought;

    [Export]
    public PlayerStats PlayerStats { get; private set; } = null!;

    [Export]
    private UnitStats defaultUnitStats = null!;

    [Node]
    private Label Traits { get; set; } = null!;

    [Node]
    private Panel Bottom { get; set; } = null!;

    [Node]
    private Label UnitName { get; set; } = null!;

    [Node]
    private Label GoldCost { get; set; } = null!;

    [Node]
    private Panel Border { get; set; } = null!;

    [Node]
    private TextureRect UnitIcon { get; set; } = null!;

    [Node]
    private Panel EmptyPlaceholder { get; set; } = null!;

    public UnitStats? UnitStats
    {
        get => _unitStats;
        set
        {
            _unitStats = value;
            UpdateUnit(_unitStats);
        }
    }

    public bool Bought { get; private set; }

    private StyleBoxFlat BorderStyle => (StyleBoxFlat)Border.GetThemeStylebox("panel");
    private StyleBoxFlat BottomStyle => (StyleBoxFlat)Bottom.GetThemeStylebox("panel");

    private static readonly Color HoverBorderColor = new("fafa82");

    private UnitStats? _unitStats;
    private Color _borderColor;

    public override void _Ready()
    {
        UpdateWithPlayerStats();

        // todo: remove these
        UnitBought += unitStats =>
        {
            GD.Print($"bought unit: {unitStats}");
            GD.Print($"gold: {PlayerStats.Gold}");
        };
    }

    public override void _EnterTree()
    {
        PlayerStats.Changed += OnPlayerStatsChanged;
        Pressed += OnPressed;
        MouseEntered += OnMouseEntered;
        MouseExited += OnMouseExited;
    }

    public override void _ExitTree()
    {
        PlayerStats.Changed -= OnPlayerStatsChanged;
        Pressed -= OnPressed;
        MouseEntered -= OnMouseEntered;
        MouseExited -= OnMouseExited;
    }

    private void UpdateUnit(UnitStats? unitStats)
    {
        if (unitStats == null)
        {
            EmptyPlaceholder.Show();
            return;
        }

        _borderColor = UnitStats.RarityColors[unitStats.Rarity];
        BorderStyle.BorderColor = _borderColor;
        BottomStyle.BgColor = _borderColor;
        UnitName.Text = unitStats.Name;
        GoldCost.Text = unitStats.GoldCost.ToString();
        var iconTexture = (AtlasTexture)UnitIcon.Texture;
        iconTexture.Region = iconTexture.Region with { Position = unitStats.SkinCoordinates * Constants.CellSize };
    }

    private void UpdateWithPlayerStats()
    {
        if (UnitStats == null)
        {
            EmptyPlaceholder.Show();
            return;
        }

        var hasEnoughGold = PlayerStats.Gold >= UnitStats.GoldCost;
        Disabled = hasEnoughGold == false;

        if (hasEnoughGold || Bought)
        {
            this.SetModulateAlpha(1);
        }
        else
        {
            this.SetModulateAlpha(0.5f);
        }
    }

    private void OnPlayerStatsChanged() => UpdateWithPlayerStats();

    private void OnPressed()
    {
        if (Bought || UnitStats == null)
            return;

        PlayerStats.Gold -= UnitStats.GoldCost;
        Bought = true;
        EmptyPlaceholder.Show();
        UnitBought?.Invoke(UnitStats);
    }

    private void OnMouseEntered()
    {
        if (Disabled)
            return;

        BorderStyle.BorderColor = HoverBorderColor;
    }

    private void OnMouseExited()
    {
        BorderStyle.BorderColor = _borderColor;
    }

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }
}