using System;
using Godot;
using GodotUtilities;
using LabAutobattler.Data.Player;
using LabAutobattler.Data.Units;

namespace LabAutobattler.UI.Shop;

[Scene]
public partial class Shop : VBoxContainer
{
    public event Action<UnitStats>? UnitBought;

    [Export]
    private PlayerStats PlayerStats { get; set; } = null!;

    [Node]
    private VBoxContainer ShopCards { get; set; } = null!;

    [Node]
    private RerollButton RerollButton { get; set; } = null!;

    public override void _EnterTree()
    {
        foreach (var unitCard in ShopCards.GetChildrenOfType<UnitCard>())
        {
            unitCard.UnitBought += OnUnitCardBought;
        }

        RerollButton.Pressed += OnRerollPressed;
    }

    public override void _ExitTree()
    {
        foreach (var unitCard in ShopCards.GetChildrenOfType<UnitCard>())
        {
            unitCard.UnitBought -= OnUnitCardBought;
        }

        RerollButton.Pressed -= OnRerollPressed;
    }

    private void OnUnitCardBought(UnitStats unitStats) => UnitBought?.Invoke(unitStats);

    private void OnRerollPressed()
    {
        GD.Print("todo: reroll units to new ones");
    }

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }
}