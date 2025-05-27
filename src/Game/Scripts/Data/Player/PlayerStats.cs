using Godot;

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

    private int _gold;
    private int _xp;
    private int _level;
}