using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;

namespace LabAutobattler.Data.Units;

[GlobalClass]
public partial class UnitPool : Resource
{
    [Export]
    public Array<UnitStats> AvailableUnits { get; private set; } = [];

    private readonly List<UnitStats> _pool = [];

    public void InitPool()
    {
        foreach (var availableUnit in AvailableUnits)
        {
            for (var i = 0; i < availableUnit.PoolCount; i++)
            {
                _pool.Add(availableUnit);
            }
        }
    }

    public UnitStats? GetRandomUnitByRarity(UnitRarity rarity)
    {
        var units = _pool.Where(unit => unit.Rarity == rarity).ToList();
        if (units.Count == 0)
            return null;

        var randomIndex = GD.RandRange(0, units.Count - 1);
        var pickedUnit = units[randomIndex];
        _pool.Remove(pickedUnit);
        return pickedUnit;
    }

    public void AddUnit(UnitStats unit)
    {
        var combinedCount = unit.CombinedUnitCount;
        unit = (UnitStats)unit.Duplicate();
        unit.Tier = 1;

        for (var i = 0; i < combinedCount; i++)
        {
            _pool.Add(unit);
        }
    }

    public override string ToString()
    {
        return string.Join(", ", _pool);
    }
}