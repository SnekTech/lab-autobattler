using Godot;
using GodotUtilities;
using LabAutobattler.Components;

namespace LabAutobattler;

[Scene]
public partial class Arena : Node2D
{
    [Node]
    private UnitMover unitMover = null!;
    [Node]
    private UnitSpawner unitSpawner = null!;

    public override void _EnterTree()
    {
        unitSpawner.UnitSpawned += unitMover.SetupUnit;
    }

    public override void _ExitTree()
    {
        unitSpawner.UnitSpawned -= unitMover.SetupUnit;
    }

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }
}