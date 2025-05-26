using Godot;

namespace LabAutobattler.Components;

public partial class OutlineHighlighter : Node
{
    [Export]
    private CanvasGroup visuals = null!;

    [Export]
    public Color OutlineColor { get; set; } = Colors.White;

    [Export(PropertyHint.Range, "1,10")]
    private float outlineThickness = 1;

    private static readonly StringName ParamLineColor = "line_color";
    private static readonly StringName ParamLineThickness = "line_thickness";

    public override void _Ready()
    {
        var shaderMaterial = (ShaderMaterial)visuals.Material;
        shaderMaterial.SetShaderParameter(ParamLineColor, OutlineColor);
    }

    public void ClearHighlight() => SetLineThickness(0);
    public void Highlight() => SetLineThickness(outlineThickness);

    private void SetLineThickness(float thickness)
    {
        var shaderMaterial = (ShaderMaterial)visuals.Material;
        shaderMaterial.SetShaderParameter(ParamLineThickness, thickness);
    }
}