using Godot;

namespace LabAutobattler;

public static class Constants
{
    public static readonly Vector2I CellSize = new(32, 32);
    public static readonly Vector2I HalfCellSize = new(16, 16);
    public static readonly Vector2I QuarterCellSize = new(8, 8);
}

public static class GroupNames
{
    public static readonly StringName Dragging = "dragging";
}

public static class InputActions
{
    public static readonly StringName CancelDrag = "cancel_drag";
    public static readonly StringName Select = "select";
}