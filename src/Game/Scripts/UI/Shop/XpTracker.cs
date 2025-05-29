using Godot;
using GodotUtilities;
using LabAutobattler.Data.Player;

namespace LabAutobattler.UI.Shop;

[Scene]
public partial class XpTracker : VBoxContainer
{
    [Export]
    private PlayerStats PlayerStats { get; set; } = null!;

    [Node]
    public ProgressBar ProgressBar { get; set; } = null!;

    [Node]
    private Label XpLabel { get; set; } = null!;

    [Node]
    private Label LevelLabel { get; set; } = null!;

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_accept"))
        {
            PlayerStats.Xp += 4;
        }
    }

    public override void _Ready()
    {
        UpdateWithPlayerStats(PlayerStats);
    }

    public override void _EnterTree()
    {
        PlayerStats.Changed += OnPlayerStatsChanged;
    }

    public override void _ExitTree()
    {
        PlayerStats.Changed -= OnPlayerStatsChanged;
    }

    private void UpdateWithPlayerStats(PlayerStats playerStats)
    {
        if (playerStats.Level < 10)
        {
            SetXpBarValues();
        }
        else
        {
            SetMaxLevelValues();
        }

        LevelLabel.Text = $"lvl: {playerStats.Level}";
        return;

        void SetXpBarValues()
        {
            var requirement = playerStats.CurrentXpRequirement;
            XpLabel.Text = $"{playerStats.Xp}/{requirement}";
            ProgressBar.Value = requirement == 0 ? 0 : playerStats.Xp / (float)requirement * 100;
        }

        void SetMaxLevelValues()
        {
            XpLabel.Text = "MAX";
            ProgressBar.Value = 100;
        }
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