using System.Collections.Generic;
using Godot;

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