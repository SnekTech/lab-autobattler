using System.Collections.Generic;
using Godot;
using LabAutobattler.Data.Units;

namespace LabAutobattler;

public static class Constants
{
    public static readonly Vector2I CellSize = new(32, 32);
    public static readonly Vector2I HalfCellSize = new(16, 16);
    public static readonly Vector2I QuarterCellSize = new(8, 8);

    public const int RerollCost = 2;
    public const int BuyXpCost = 4;

    public static readonly Dictionary<int, int> XpRequirements = new()
    {
        [1] = 0,
        [2] = 2,
        [3] = 2,
        [4] = 6,
        [5] = 10,
        [6] = 20,
        [7] = 36,
        [8] = 48,
        [9] = 76,
        [10] = 76
    };
    public static readonly Dictionary<int, UnitRarity[]> RollRarities = new()
    {
        [1] = [UnitRarity.Common],
        [2] = [UnitRarity.Common],
        [3] = [UnitRarity.Common, UnitRarity.Uncommon],
        [4] = [UnitRarity.Common, UnitRarity.Uncommon, UnitRarity.Rare],
        [5] = [UnitRarity.Common, UnitRarity.Uncommon, UnitRarity.Rare],
        [6] = [UnitRarity.Common, UnitRarity.Uncommon, UnitRarity.Rare],
        [7] = [UnitRarity.Common, UnitRarity.Uncommon, UnitRarity.Rare, UnitRarity.Legendary],
        [8] = [UnitRarity.Common, UnitRarity.Uncommon, UnitRarity.Rare, UnitRarity.Legendary],
        [9] = [UnitRarity.Common, UnitRarity.Uncommon, UnitRarity.Rare, UnitRarity.Legendary],
        [10] = [UnitRarity.Common, UnitRarity.Uncommon, UnitRarity.Rare, UnitRarity.Legendary]
    };
    public static readonly Dictionary<int, float[]> RollChances = new()
    {
        [1] = [1f],
        [2] = [1f],
        [3] = [7.5f, 2.5f],
        [4] = [6.5f, 3.0f, 0.5f],
        [5] = [5.0f, 3.5f, 1.5f],
        [6] = [4.0f, 4.0f, 2.0f],
        [7] = [2.75f, 4.0f, 3.24f, 0.1f],
        [8] = [2.5f, 3.75f, 3.45f, 0.3f],
        [9] = [1.75f, 2.75f, 4.5f, 1.0f],
        [10] = [1.0f, 2.0f, 4.5f, 2.5f]
    };
}

public static class GroupNames
{
    public static readonly StringName Dragging = "dragging";
    public static readonly StringName Units = "units";
}

public static class InputActions
{
    public static readonly StringName CancelDrag = "cancel_drag";
    public static readonly StringName Select = "select";
    public static readonly StringName QuickSell = "quick_sell";
}