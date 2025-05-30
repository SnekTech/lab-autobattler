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

    private const int UnitCardSlotCount = 5;
    private static readonly PackedScene UnitCardScene = GD.Load<PackedScene>("res://Scenes/shop/UnitCard.tscn");

    [Export]
    private PlayerStats PlayerStats { get; set; } = null!;

    [Export]
    private UnitPool UnitPool { get; set; } = null!;

    [Node]
    private VBoxContainer ShopCards { get; set; } = null!;

    [Node]
    private RerollButton RerollButton { get; set; } = null!;

    public override void _EnterTree()
    {
        RerollButton.Pressed += OnRerollPressed;
    }

    public override void _Ready()
    {
        UnitPool.InitPool();

        foreach (var unitCard in ShopCards.GetChildrenOfType<UnitCard>())
        {
            unitCard.QueueFree();
        }

        RollUnits();
    }

    public override void _ExitTree()
    {
        foreach (var unitCard in ShopCards.GetChildrenOfType<UnitCard>())
        {
            unitCard.UnitBought -= OnUnitCardBought;
        }

        RerollButton.Pressed -= OnRerollPressed;
    }

    private void RollUnits()
    {
        for (var i = 0; i < UnitCardSlotCount; i++)
        {
            var rarity = PlayerStats.GetRandomRarityForLevel();
            var newCard = UnitCardScene.Instantiate<UnitCard>();
            newCard.UnitStats = UnitPool.GetRandomUnitByRarity(rarity);
            newCard.UnitBought += OnUnitCardBought;
            ShopCards.AddChild(newCard);
        }
    }

    private void PutBackRemainingToPool()
    {
        foreach (var unitCard in ShopCards.GetChildrenOfType<UnitCard>())
        {
            if (unitCard.Bought == false && unitCard.UnitStats != null)
            {
                UnitPool.AddUnit(unitCard.UnitStats);
            }

            unitCard.QueueFree();
        }
    }

    private void OnUnitCardBought(UnitStats unitStats) => UnitBought?.Invoke(unitStats);

    private void OnRerollPressed()
    {
        PutBackRemainingToPool();
        RollUnits();
    }

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }
}