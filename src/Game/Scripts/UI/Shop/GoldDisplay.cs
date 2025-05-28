using Godot;
using GodotUtilities;
using LabAutobattler.Data.Player;

namespace LabAutobattler.UI.Shop;

[Scene]
public partial class GoldDisplay : HBoxContainer
{
    [Export]
    private PlayerStats PlayerStats { get; set; } = null!;

    [Node]
    private Label Gold { get; set; } = null!;

    public override void _Ready()
    {
        UpdateContent(PlayerStats);
    }

    public override void _EnterTree()
    {
        PlayerStats.Changed += OnPlayerStatsChanged;
    }

    public override void _ExitTree()
    {
        PlayerStats.Changed -= OnPlayerStatsChanged;
    }

    private void OnPlayerStatsChanged() => UpdateContent(PlayerStats);

    private void UpdateContent(PlayerStats stats)
    {
        Gold.Text = stats.Gold.ToString();
    }

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }
}