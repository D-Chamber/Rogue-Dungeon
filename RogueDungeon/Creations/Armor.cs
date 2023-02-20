using RogueDungeon.Interfaces;

namespace RogueDungeon.Creations;

public class Armor : IArmor
{
    private string _name;
    private float _weight;
    private float _value;
    private int _quantity;
    private IItem _decorator;
    private bool _isDecorator;
    private int[] _armorMod = new int[2];
    private float _speedMod;
    
    public string ArmorType { get; set; }

    public int[] ArmorMod
    {
        get => _armorMod;
        set => _armorMod = value;
    }

    public float SpeedMod
    {
        get
        {
            if (ArmorType == "Heavy")
            {
                return .70f;
            }
            else if (ArmorType == "Medium")
            {
                return .85f;
            }
            else
            {
                return 1f;
            }
        }
    }

    public Armor(string name = "No Name", float weight = 1f, float value = 1f, bool isDecorator = false)
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
                   "Armor: " + ArmorMod[0] + "-" + ArmorMod[1] + "\n";
        }
    }
}