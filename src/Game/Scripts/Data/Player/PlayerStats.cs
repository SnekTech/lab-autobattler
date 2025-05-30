using System.Linq;
using Godot;
using LabAutobattler.Data.Units;

namespace LabAutobattler.Data.Player;

[GlobalClass]
public partial class PlayerStats : Resource
{
    [Export(PropertyHint.Range, "0,99")]
    public int Gold
    {
        get => _gold;
        set
        {
            _gold = value;
            EmitChanged();
        }
    }

    [Export(PropertyHint.Range, "0,99")]
    public int Xp
    {
        get => _xp;
        set
        {
            _xp = value;
            EmitChanged();

            if (Level == 10)
                return;

            var xpRequirement = CurrentXpRequirement;
            while (Level < 10 && _xp >= xpRequirement)
            {
                Level++;
                _xp -= xpRequirement;
                xpRequirement = CurrentXpRequirement;
                EmitChanged();
            }
        }
    }

    [Export(PropertyHint.Range, "1,10")]
    public int Level
    {
        get => _level;
        set
        {
            _level = value;
            EmitChanged();
        }
    }

    public int CurrentXpRequirement
    {
        get
        {
            var nextLevel = Mathf.Clamp(Level + 1, 1, 10);
            return Constants.XpRequirements[nextLevel];
        }
    }

    public UnitRarity GetRandomRarityForLevel()
    {
        var rng = new RandomNumberGenerator();
        var array = Constants.RollRarities[Level];
        var weights = Constants.RollChances[Level];

        return array[rng.RandWeighted(weights)];
    }

    private int _gold;
    private int _xp;
    private int _level;
}