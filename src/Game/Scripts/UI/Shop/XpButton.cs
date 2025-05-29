using Godot;
using GodotUtilities;
using LabAutobattler.Data.Player;

namespace LabAutobattler.UI.Shop;

[Scene]
public partial class XpButton : Button
{
    [Export]
    private PlayerStats PlayerStats { get; set; } = null!;

    [Node]
    private VBoxContainer VBoxContainer { get; set; } = null!;

    public override void _EnterTree()
    {
        PlayerStats.Changed += OnPlayerStatsChanged;
        Pressed += OnPressed;
    }

    public override void _ExitTree()
    {
        PlayerStats.Changed -= OnPlayerStatsChanged;
        Pressed -= OnPressed;
    }

    private void UpdateWithPlayerStats()
    {
        var hasEnoughGold = PlayerStats.Gold >= Constants.BuyXpCost;
        var reachedMaxLevel = PlayerStats.Level == 10;
        Disabled = hasEnoughGold == false || reachedMaxLevel;

        if (Disabled)
        {
            VBoxContainer.SetModulateAlpha(0.5f);
        }
        else
        {
            VBoxContainer.SetModulateAlpha(1);
        }
    }

    private void OnPlayerStatsChanged() => UpdateWithPlayerStats();

    private void OnPressed()
    {
        PlayerStats.Gold -= 4;
        PlayerStats.Xp += 4;
    }

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }
}