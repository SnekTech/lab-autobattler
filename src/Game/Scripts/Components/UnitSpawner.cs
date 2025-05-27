using System;
using Godot;
using LabAutobattler.Data.Units;
using LabAutobattler.UnitManage;

namespace LabAutobattler.Components;

public partial class UnitSpawner : Node
{
    public event Action<Unit>? UnitSpawned;

    private static readonly PackedScene UnitScene = GD.Load<PackedScene>("res://Scenes/unit/Unit.tscn");

    [Export]
    public PlayArea Bench { get; private set; } = null!;

    [Export]
    public PlayArea GameArea { get; private set; } = null!;

    private PlayArea? GetFirstAvailableArea()
    {
        if (Bench.UnitGrid.IsGridFull() == false)
            return Bench;
        if (GameArea.UnitGrid.IsGridFull() == false)
            return GameArea;

        return null;
    }

    public void SpawnUnit(UnitStats unitStats)
    {
        var area = GetFirstAvailableArea();
        if (area == null)
        {
            // todo: use in-game popup message in the future
            GD.PrintErr("No available space to add unit to!");
            return;
        }

        var newUnit = UnitScene.Instantiate<Unit>();
        var tile = area.UnitGrid.GetFirstEmptyTile();
        area.UnitGrid.AddChild(newUnit);
        area.UnitGrid.AddUnit(tile, newUnit);
        area.SnapUnitToTile(newUnit, tile);
        newUnit.Stats = unitStats;
        UnitSpawned?.Invoke(newUnit);
    }
}