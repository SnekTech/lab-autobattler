using Godot;
using Godot.Collections;
using GodotUtilities;
using LabAutobattler.UnitManage;

namespace LabAutobattler.Components;

public partial class UnitMover : Node
{
    [Export]
    public Array<PlayArea> PlayAreas { get; private set; } = [];

    public override void _Ready()
    {
        var units = GetTree().GetNodesInGroup<Unit>(GroupNames.Units);
        foreach (var unit in units)
        {
            SetupUnit(unit);
        }
    }

    private void SetupUnit(Unit unit)
    {
        var unitDnd = unit.DragAndDrop;
        unitDnd.DragStarted += () => OnUnitDragStarted(unit);
        unitDnd.DragCanceled += startingPosition => OnUnitDragCanceled(startingPosition, unit);
        unitDnd.Dropped += startingPosition => OnUnitDropped(startingPosition, unit);
    }

    private void SetHighlighters(bool enabled)
    {
        foreach (var playArea in PlayAreas)
        {
            playArea.TileHighlighter.Enabled = enabled;
        }
    }

    private int GetPlayAreaIndexForPosition(Vector2 global)
    {
        var areaIndex = -1;
        for (var i = 0; i < PlayAreas.Count; i++)
        {
            var playArea = PlayAreas[i];
            var tile = playArea.GetTileFromGlobal(global);
            if (playArea.IsTileInBounds(tile))
            {
                areaIndex = i;
            }
        }

        return areaIndex;
    }

    public void ResetUnitToStartingPosition(Vector2 startingPosition, Unit unit)
    {
        var index = GetPlayAreaIndexForPosition(startingPosition);
        if (index < 0)
        {
            GD.PrintErr($"can't find {nameof(PlayArea)} at starting position {startingPosition}");
            return;
        }

        var playArea = PlayAreas[index];
        var tile = playArea.GetTileFromGlobal(startingPosition);

        unit.ResetAfterDragging(startingPosition);
        playArea.UnitGrid.AddUnit(tile, unit);
    }

    private void MoveUnit(Unit unit, PlayArea playArea, Vector2I tile)
    {
        playArea.UnitGrid.AddUnit(tile, unit);
        unit.GlobalPosition = playArea.GetGlobalFromTile(tile) - Constants.HalfCellSize;
        unit.Reparent(playArea.UnitGrid);
    }

    private void OnUnitDragStarted(Unit unit)
    {
        SetHighlighters(true);

        var i = GetPlayAreaIndexForPosition(unit.GlobalPosition);
        if (i >= 0)
        {
            var tile = PlayAreas[i].GetTileFromGlobal(unit.GlobalPosition);
            PlayAreas[i].UnitGrid.RemoveUnit(tile);
        }
    }

    private void OnUnitDragCanceled(Vector2 startingPosition, Unit unit)
    {
        SetHighlighters(false);
        ResetUnitToStartingPosition(startingPosition, unit);
    }

    private void OnUnitDropped(Vector2 startingPosition, Unit unit)
    {
        SetHighlighters(false);
        var oldAreaIndex = GetPlayAreaIndexForPosition(startingPosition);
        var dropAreaIndex = GetPlayAreaIndexForPosition(unit.GetGlobalMousePosition());

        if (dropAreaIndex == -1)
        {
            ResetUnitToStartingPosition(startingPosition, unit);
            return;
        }

        var oldArea = PlayAreas[oldAreaIndex];
        var oldTile = oldArea.GetTileFromGlobal(startingPosition);
        var newArea = PlayAreas[dropAreaIndex];
        var newTile = newArea.GetHoveredTile();

        var shouldSwapUnits = newArea.UnitGrid.IsTileOccupied(newTile);
        if (shouldSwapUnits)
        {
            var oldUnit = newArea.UnitGrid.GetUnitAt(newTile)!;
            newArea.UnitGrid.RemoveUnit(newTile);
            MoveUnit(oldUnit, oldArea, oldTile);
        }

        MoveUnit(unit, newArea, newTile);
    }
}