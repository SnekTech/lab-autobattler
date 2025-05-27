using Godot;
using GodotUtilities;
using LabAutobattler.Components;
using LabAutobattler.Data.Units;

namespace LabAutobattler.UnitManage;

[Scene]
public partial class Unit : Area2D
{
    [Export]
    private UnitStats defaultStats = null!;

    [Node]
    private Sprite2D skin = null!;

    [Node]
    private ProgressBar healthBar = null!;

    [Node]
    private ProgressBar manaBar = null!;

    [Node]
    public DragAndDrop DragAndDrop { get; private set; } = null!;

    [Node]
    private VelocityBasedRotation velocityBasedRotation = null!;

    [Node]
    private OutlineHighlighter outlineHighlighter = null!;

    private UnitStats _stats = null!;

    public UnitStats Stats
    {
        get => _stats;
        set
        {
            _stats = value;
            UpdateWithStats(_stats);
        }
    }

    public override void _Ready()
    {
        Stats = defaultStats;
    }

    public override void _EnterTree()
    {
        MouseEntered += OnMouseEntered;
        MouseExited += OnMouseExited;
        DragAndDrop.DragStarted += OnDragStart;
        DragAndDrop.DragCanceled += OnDragCanceled;
    }

    public override void _ExitTree()
    {
        MouseEntered -= OnMouseEntered;
        MouseExited -= OnMouseExited;
        DragAndDrop.DragStarted -= OnDragStart;
        DragAndDrop.DragCanceled -= OnDragCanceled;
    }

    private void OnMouseEntered()
    {
        if (DragAndDrop.Dragging)
            return;

        outlineHighlighter.Highlight();
        ZIndex = 1;
    }

    private void OnMouseExited()
    {
        if (DragAndDrop.Dragging)
            return;

        outlineHighlighter.ClearHighlight();
        ZIndex = 0;
    }

    private void OnDragStart()
    {
        velocityBasedRotation.Enabled = true;
    }

    private void OnDragCanceled(Vector2 startingPosition)
    {
        ResetAfterDragging(startingPosition);
    }

    public void ResetAfterDragging(Vector2 startingPosition)
    {
        velocityBasedRotation.Enabled = false;
        GlobalPosition = startingPosition;
    }

    private void UpdateWithStats(UnitStats newStats)
    {
        var (skinX, skinY) = newStats.SkinCoordinates;
        skin.RegionRect = skin.RegionRect with { Position = new Vector2(skinX, skinY) * Constants.CellSize };
    }

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }
}