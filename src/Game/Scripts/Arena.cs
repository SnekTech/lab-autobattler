using Godot;
using GodotUtilities;
using LabAutobattler.Components;
using LabAutobattler.UnitManage;

namespace LabAutobattler;

[Scene]
public partial class Arena : Node2D
{
    [Node]
    private UnitMover unitMover = null!;
    [Node]
    private UnitSpawner unitSpawner = null!;
    [Node]
    private SellPortal sellPortal = null!;

    public override void _EnterTree()
    {
        unitSpawner.UnitSpawned += OnUnitSpawned;
    }

    public override void _ExitTree()
    {
        unitSpawner.UnitSpawned -= OnUnitSpawned;
    }

    private void OnUnitSpawned(Unit unit)
    {
        unitMover.SetupUnit(unit);
        sellPortal.SetupUnit(unit);
    }

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }
}