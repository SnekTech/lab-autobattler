using Godot;

namespace LabAutobattler.Components;

public partial class TileHighlighter : Node
{
    [Export]
    private bool enabled = true;
    [Export]
    private PlayArea playArea = null!;
    [Export]
    private TileMapLayer highlightLayer = null!;
    [Export]
    private Vector2I tile;

    private int _sourceId;

    public bool Enabled
    {
        get => enabled;
        set
        {
            enabled = value;
            if (enabled == false)
            {
                highlightLayer.Clear();
            }
        }
    }

    public override void _Ready()
    {
        _sourceId = playArea.TileSet.GetSourceId(0);
    }

    public override void _Process(double delta)
    {
        if (enabled == false)
            return;

        var selectedTile = playArea.GetHoveredTile();
        if (playArea.IsTileInBounds(selectedTile) == false)
        {
            highlightLayer.Clear();
            return;
        }
        
        UpdateTile(selectedTile);
    }

    private void UpdateTile(Vector2I selectedTile)
    {
        highlightLayer.Clear();
        highlightLayer.SetCell(selectedTile, _sourceId, tile);
    }
}