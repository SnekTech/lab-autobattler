using Godot;
using GodotUtilities;
using LabAutobattler.Components;
using LabAutobattler.Data.Units;
using LabAutobattler.UI.Shop;
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
    [Node]
    private Shop shop = null!;

    public override void _EnterTree()
    {
        unitSpawner.UnitSpawned += OnUnitSpawned;
        shop.UnitBought += OnUnitBought;
    }

    public override void _ExitTree()
    {
        unitSpawner.UnitSpawned -= OnUnitSpawned;
        shop.UnitBought -= OnUnitBought;
    }

    private void OnUnitBought(UnitStats unitStats) => unitSpawner.SpawnUnit(unitStats);

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