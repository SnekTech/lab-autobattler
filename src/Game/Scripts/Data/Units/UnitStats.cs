using Godot;
using Godot.Collections;

namespace LabAutobattler.Data.Units;

[GlobalClass]
public partial class UnitStats : Resource
{
    public static readonly Dictionary<UnitRarity, Color> RarityColors = new()
    {
        [UnitRarity.Common] = new Color("124a2e"),
        [UnitRarity.Uncommon] = new Color("1c527c"),
        [UnitRarity.Rare] = new Color("ab0979"),
        [UnitRarity.Legendary] = new Color("ea940b"),
    };

    [Export]
    public string Name { get; private set; } = "John Doe";

    [ExportCategory("Data")]
    [Export]
    public UnitRarity Rarity { get; private set; } = UnitRarity.Common;

    [Export]
    public int GoldCost { get; private set; } = 1;

    [ExportCategory("Visuals")]
    [Export]
    public Vector2I SkinCoordinates { get; private set; }

    public override string ToString()
    {
        return Name;
    }
}