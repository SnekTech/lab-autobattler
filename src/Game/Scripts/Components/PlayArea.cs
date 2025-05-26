using Godot;

namespace LabAutobattler.Components;

public partial class PlayArea : TileMapLayer
{
    [Export]
    public UnitGrid UnitGrid { get; private set; } = null!;

    private Rect2I _bounds;

    public override void _Ready()
    {
        _bounds = new Rect2I(Vector2I.Zero, UnitGrid.Size);
    }

    public Vector2I GetTileFromGlobal(Vector2 globalPosition) => LocalToMap(ToLocal(globalPosition));

    public Vector2 GetGlobalFromTile(Vector2I tile) => ToGlobal(MapToLocal(tile));

    public Vector2I GetHoveredTile() => LocalToMap(GetLocalMousePosition());

    public bool IsTileInBounds(Vector2I tile) => _bounds.HasPoint(tile);
}