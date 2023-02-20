using RogueDungeon.Interfaces;

namespace RogueDungeon.Creations;

// this class is going to be used to create the consumable items and the decorations
public class Consumable : IConsumable
{
    private IItem _decorator;
    private int _quantity;
    private float _value;
    private float _weight;

    public string Name { get; }
    public string LongName { get { return Name + (_decorator != null ? ", " + _decorator.LongName : ""); } }
    public float Weight 
    {
        get
        {
            return (_weight * Count) + (_decorator != null ? _decorator.Weight : 0f);
        }
        private set => _weight = value;
    }

    public float Value
    {
        get
        {
            return (_value * Count) + (_decorator != null ? _decorator.Value : 0f);
        }
        private set => _value = value;
    }
    
    public int Count { get; set; }
    public bool IsDecorator { get; }
    public int[] Modifier { get; set; } = new int[2];

    /*
     * This is the constructor for the consumable class
     * I learned that you can implement a "designated constructor" using this technique
     */
    public Consumable(string name = "" , float weight = 0f, float value = 0f, int count = 0, bool isDecorator = false)
    {
        Name = name;
        Weight = weight;
        Value = value;
        Count = count;
        _decorator = null;
        IsDecorator = isDecorator;
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

    public string Description 
    { 
        get
        {
            return LongName + (Count > 1 ? $" x{Count}" : "") + ":\n__________________\n" +
                   "Weight: " + Weight + "\nValue: " + Value + "\n";
        }
    }
}