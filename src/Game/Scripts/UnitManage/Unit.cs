using Godot;
using GodotUtilities;
using LabAutobattler.Data.Units;

namespace LabAutobattler.UnitManage;

[Scene]
public partial class Unit : Area2D
{
    [Export]
    private UnitStats defaultStats = null!;

    [Node]
    private Sprite2D skin = null!;

    [Node]
    private ProgressBar healthBar = null!;

    [Node]
    private ProgressBar manaBar = null!;

    private UnitStats? _stats;

    public UnitStats Stats
    {
        get => _stats ?? defaultStats;
        set
        {
            _stats = value;
            UpdateWithStats(_stats);
        }
    }
    
    public override void _Ready()
    {
        Stats = defaultStats;
    }

    private void UpdateWithStats(UnitStats newStats)
    {
        var (skinX, skinY) = newStats.SkinCoordinates;
        skin.RegionRect = skin.RegionRect with { Position = new Vector2(skinX, skinY) * Constants.CellSize };
    }

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }
}