using Godot;
using GodotUtilities;
using LabAutobattler.Data.Player;

namespace LabAutobattler.UI.Shop;

[Scene]
public partial class RerollButton : Button
{
    [Export]
    private PlayerStats PlayerStats { get; set; } = null!;

    [Node]
    private HBoxContainer hBoxContainer = null!;

    public override void _Ready()
    {
        UpdateWithPlayerStats(PlayerStats);
    }

    public override void _EnterTree()
    {
        Pressed += OnPressed;
        PlayerStats.Changed += OnPlayerStatsChanged;
    }

    public override void _ExitTree()
    {
        Pressed -= OnPressed;
        PlayerStats.Changed -= OnPlayerStatsChanged;
    }

    private void UpdateWithPlayerStats(PlayerStats playerStats)
    {
        var hasEnoughGold = playerStats.Gold >= Constants.RerollCost;
        Disabled = hasEnoughGold == false;

        if (hasEnoughGold)
        {
            hBoxContainer.SetModulateAlpha(1);
        }
        else
        {
            hBoxContainer.SetModulateAlpha(0.5f);
        }
    }

    private void OnPressed()
    {
        PlayerStats.Gold -= 2;
    }

    private void OnPlayerStatsChanged() => UpdateWithPlayerStats(PlayerStats);

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }
}