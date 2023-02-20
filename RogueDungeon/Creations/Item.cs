using System.Runtime.CompilerServices;
using RogueDungeon.Interfaces;

namespace RogueDungeon;


// generic Items class
public class Item : IItem
{
    private string _name;
    private float _weight;
    private float _value;
    private int _quantity;
    private IItem _decorator;
    private bool _isDecorator;

    public Item() : this("NoName")
    {
    }

    public Item(string name) : this(name, 1f)
    {
    }

    public Item(string name, float weight) : this(name, weight, 1f)
    {
    }

    public Item(string name, float weight, float value) : this(name, weight, value, 1)
    {
    }

    public Item(string name, float weight, float value, int count) : this(name, weight, value, count, false)
    {
    }
    
    public Item(string name, float weight, float value, int count, bool isDecorator)
    {
        _name = name;
        _weight = weight;
        _value = value;
        _quantity = count;
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
                               "Weight: " + Weight + "\nValue: " + Value + "\n";
        }
    }
}