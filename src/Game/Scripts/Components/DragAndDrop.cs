using System;
using Godot;

namespace LabAutobattler.Components;

public partial class DragAndDrop : Node
{
    public event Action<Vector2>? DragCanceled;
    public event Action? DragStarted;
    public event Action<Vector2>? Dropped;

    [Export]
    public bool Enabled { get; set; } = true;

    [Export]
    public Area2D Target { get; private set; } = null!;

    private Vector2 _startingPosition;
    private Vector2 _offset;

    public bool Dragging { get; private set; }

    public override void _Process(double delta)
    {
        if (Dragging)
        {
            Target.GlobalPosition = Target.GetGlobalMousePosition() + _offset;
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (Dragging && @event.IsActionPressed(InputActions.CancelDrag))
        {
            CancelDragging();
        }
        else if (Dragging && @event.IsActionReleased(InputActions.Select))
        {
            Drop();
        }
    }

    public override void _EnterTree()
    {
        Target.InputEvent += OnTargetInputEvent;
    }

    public override void _ExitTree()
    {
        Target.InputEvent -= OnTargetInputEvent;
    }

    private void EndDragging()
    {
        Dragging = false;
        Target.RemoveFromGroup(GroupNames.Dragging);
        Target.ZIndex = 0;
    }

    private void CancelDragging()
    {
        EndDragging();
        DragCanceled?.Invoke(_startingPosition);
    }

    private void StartDragging()
    {
        Dragging = true;
        _startingPosition = Target.GlobalPosition;
        Target.AddToGroup(GroupNames.Dragging);
        Target.ZIndex = 99;
        _offset = Target.GlobalPosition - Target.GetGlobalMousePosition();
        DragStarted?.Invoke();
    }

    private void Drop()
    {
        EndDragging();
        Dropped?.Invoke(_startingPosition);
    }

    private void OnTargetInputEvent(Node viewport, InputEvent @event, long shapeIdx)
    {
        if (Enabled == false)
            return;

        var draggingObject = GetTree().GetFirstNodeInGroup(GroupNames.Dragging);

        if (Dragging == false && draggingObject != null)
            return; // is dragging another object

        if (Dragging == false && @event.IsActionPressed(InputActions.Select))
        {
            StartDragging();
        }
    }
}