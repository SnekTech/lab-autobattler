using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using LabAutobattler.UnitManage;

namespace LabAutobattler.Components;

public partial class UnitGrid : Node2D
{
    public event Action? UnitGridChanged;

    [Export]
    public Vector2I Size { get; private set; }

    private Godot.Collections.Dictionary<Vector2I, Unit?> _units = [];

    public override void _Ready()
    {
        InitUnits();
        return;

        void InitUnits()
        {
            for (var i = 0; i < Size.X; i++)
            {
                for (var j = 0; j < Size.Y; j++)
                {
                    _units[new Vector2I(i, j)] = null;
                }
            }
        }
    }

    public void AddUnit(Vector2I index, Unit unit)
    {
        _units[index] = unit;
        UnitGridChanged?.Invoke();
    }

    public bool IsTileOccupied(Vector2I tile) => _units[tile] != null;

    public bool IsGridFull() => _units.Keys.All(IsTileOccupied);

    public Vector2I GetFirstEmptyTile()
    {
        foreach (var index in _units.Keys)
        {
            if (IsTileOccupied(index) == false)
                return index;
        }

        return new Vector2I(-1, -1);
    }

    public List<Unit> GetAllUnits()
    {
        return _units.Values.OfType<Unit>().ToList();
    }
}