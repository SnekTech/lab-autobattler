using System;
using System.Linq;
using Godot;
using System.Collections.Generic;
using System.Text;
using LabAutobattler.UnitManage;

namespace LabAutobattler.Components;

public partial class UnitGrid : Node2D
{
    public event Action? UnitGridChanged;

    [Export]
    public Vector2I Size { get; private set; }

    private readonly Dictionary<Vector2I, Unit?> _units = [];
    private readonly Dictionary<Vector2I, Action> _unitExitedDelegates = [];

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

    public void AddUnit(Vector2I tile, Unit unit)
    {
        _units[tile] = unit;
        _unitExitedDelegates[tile] = () => OnUnitExitedTree(unit, tile);
        unit.TreeExited += _unitExitedDelegates[tile];
        UnitGridChanged?.Invoke();
    }

    public void RemoveUnit(Vector2I tile)
    {
        var unit = _units[tile];
        if (unit == null)
            return;

        unit.TreeExited -= _unitExitedDelegates[tile];
        _units[tile] = null;
        UnitGridChanged?.Invoke();
    }

    public Unit? GetUnitAt(Vector2I tile) => _units[tile];

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
        return [.._units.Values.OfType<Unit>()];
    }

    private void OnUnitExitedTree(Unit unit, Vector2I tile)
    {
        if (unit.IsQueuedForDeletion() == false)
            return;

        _units[tile] = null;
        UnitGridChanged?.Invoke();
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("units dictionary:").AppendLine();

        foreach (var (tile, unit) in _units)
        {
            var unitString = unit == null ? "null" : unit.ToString();
            sb.Append($"[{tile}]: {unitString}, ");
        }

        sb.AppendLine().Append("----------").AppendLine();

        sb.Append("get all units: ").AppendLine();
        var units = GetAllUnits();
        sb.Append('[').Append(string.Join(", ", units)).Append(']').AppendLine();

        return sb.ToString();
    }
}