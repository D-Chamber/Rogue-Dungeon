using RogueDungeon.Interfaces;

namespace RogueDungeon.Creations;

public class Weapon : IWeapon
{
    private string _name;
    private float _weight;
    private float _value;
    private int _quantity;
    private IItem _decorator;
    private bool _isDecorator;
    private int[] _damageMod = new int[2];

    public int[] DamageMod
    {
        get => _damageMod;
        set => _damageMod = value;
    }

    /*
     * Class makes a weapon object that can be used in combat and
     * placed in inventory extends IItem interface
     * Using the IWeapon interface
     */
    public Weapon(string name = "No Name", float weight = 1f, float value = 1f, bool isDecorator = false)
    {
        _name = name;
        _weight = weight;
        _value = value;
        _quantity = 1;
        _decorator = null;
        _isDecorator = isDecorator;
    }

    public string Name{ get { return _name; } }
    
    public string LongName { get { return Name + (_decorator != null ? ", " + _decorator.LongName : ""); } }
    
    public float Weight
    {
        get { return _weight + (_decorator != null ? _decorator.Weight : 0f); }
    }
    
    public float Value
    {
        get { return _value + (_decorator != null ? _decorator.Value : 0f); }
    }
    public bool IsDecorator
    {
        get { return _isDecorator; }
        set => _isDecorator = value;
    }

    public void AddDecorator(IItem decorator)
    {
        if (decorator.IsDecorator)
        {
            if (_decorator == null)
            { 
                _decorator = decorator;
            }
            else
            {
                _decorator.AddDecorator(decorator);
            }
        }
    }

    public int Count
    {
        get => _quantity;
        set => _quantity = value;
    }
    
    public string Description 
    { 
        get
        {
            return LongName + (Count > 1 ? $" x{Count}" : "") + ":\n__________________\n" +
                               "Weight: " + Weight + "\nValue: " + Value + "\n" +
                               "Damage: " + DamageMod[0] + "-" + DamageMod[1] + "\n";
        }
    }
}