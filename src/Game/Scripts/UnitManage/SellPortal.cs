using Godot;
using GodotUtilities;
using LabAutobattler.Components;
using LabAutobattler.Data.Player;
using LabAutobattler.Data.Units;

namespace LabAutobattler.UnitManage;

[Scene]
public partial class SellPortal : Area2D
{
    [Export]
    private PlayerStats playerStats = null!;
    [Export]
    private UnitPool unitPool = null!;

    [Node]
    private HBoxContainer gold = null!;
    [Node]
    private Label goldLabel = null!;
    [Node]
    private OutlineHighlighter outlineHighlighter = null!;

    private Unit? _currentUnit;

    public override void _Ready()
    {
        var units = GetTree().GetNodesInGroup<Unit>(GroupNames.Units);
        foreach (var unit in units)
        {
            SetupUnit(unit);
        }
    }

    public override void _EnterTree()
    {
        AreaEntered += OnAreaEntered;
        AreaExited += OnAreaExited;
    }

    public override void _ExitTree()
    {
        AreaEntered -= OnAreaEntered;
        AreaExited -= OnAreaExited;
    }

    public void SetupUnit(Unit unit)
    {
        unit.DragAndDrop.Dropped += _ => OnUnitDropped(unit);
        unit.QuickSellPressed += () => SellUnit(unit);
    }

    private void OnUnitDropped(Unit unit)
    {
        if (unit == _currentUnit)
        {
            SellUnit(unit);
        }
    }

    private void SellUnit(Unit unit)
    {
        playerStats.Gold += unit.Stats.GoldValue;

        // todo: give items back to item pool

        unitPool.AddUnit(unit.Stats);

        unit.QueueFree();
    }

    private void OnAreaEntered(Area2D area)
    {
        if (area is not Unit unit)
            return;

        _currentUnit = unit;
        outlineHighlighter.Highlight();
        goldLabel.Text = unit.Stats.GoldValue.ToString();
        gold.Show();
    }

    private void OnAreaExited(Area2D area)
    {
        if (area is not Unit unit)
            return;

        if (unit == _currentUnit)
        {
            _currentUnit = null;
        }

        outlineHighlighter.ClearHighlight();
        gold.Hide();
    }

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }
}