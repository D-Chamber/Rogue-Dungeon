using RogueDungeon.Interfaces;

namespace RogueDungeon.Creations;

public class Chest : IItemContainer
{
    private float _weight;
    private float _value;

    public string Name { get; private set; }
    public string LongName { get; private set; }

    public float Weight
    {
        get
        {
            return _items.Values.Sum(item => item.Weight);
        }
        private set => _weight = value;
    }

    public float Value {
        get
        {
            return _items.Values.Sum(item => item.Value);
        }
        private set => _value = value; 
    }
    
    public int Count
    {
        get => _items.Count;
        set { }
    }

    public string Description
    {
        get
        {
            if (_items.Count == 0) return "<.>\n";
            
            LongName = Name + $" (Weight: {Weight} Value: {Value}) " + "\n_____________________________\n\n";
            foreach (var item in _items.Values)
            {
                LongName += item.Description + "\n";
            }
            return LongName;
        }
    }

    public bool IsDecorator => false;
    private readonly Dictionary<string, IItem> _items;


    public Chest() : this("Chest")
    {
    }

    public Chest(string name) : this(name, 0f)
    {
    }

    public Chest(string name, float weight) : this(name, weight, 0f)
    {
    }

    private Chest(string name, float weight, float value)
    {
        Name = name;
        Weight = weight;
        Value = value;
        _items = new Dictionary<string, IItem>();
    }

    public void AddDecorator(IItem decorator)
    {
        // Do Nothing
        return;
    }

    public void AddItem(IItem item) => _items.Add(item.Name, item);

    public IItem RemoveItem(string itemName)
    {
        _items.Remove(itemName, out var itemToReturn);
        return itemToReturn;
    }
}

public class Inventory : IItemContainer
{
    private readonly Dictionary<string, IItem> _items;
    private float _weight;
    private float _value;
    
    public string Name { get; private set; }
    public string LongName { get; private set; }
    public float Weight
    {
        get
        {
            float weight = 0;
            foreach (var item in _items.Values)
            {
                weight += item.Weight;
            }

            return weight;
        }
        private set => _weight = value;
    }

    public float Value 
    {
        get
        {
            float value = 0;
            foreach (var item in _items.Values)
            {
                value += item.Value;
            }

            return value;
        }
        private set => _value = value; 
    }

    public int Count
    {
        get => _items.Count;
        set { }
    }

    public string Description
    {
        get
        {
            if (_items.Count == 0) return "<.>\n";
            LongName = Name + $" (Weight: {Weight} Value: {Value}) " + "\n_____________________________\n\n";
            foreach (var item in _items.Values)
            {
                LongName += item.Description + "\n";
            }

            return LongName;
        }
    }

    public bool IsDecorator { get; }
    
    public Inventory() : this("Inventory")
    {
    }

    public Inventory(string name) : this(name, 0f)
    {
    }

    public Inventory(string name, float weight) : this(name, weight, 0f)
    {
    }

    private Inventory(string name, float weight, float value)
    {
        Name = name;
        Weight = weight;
        Value = value;
        _items = new Dictionary<string, IItem>();
        IsDecorator = false;
    }


    public void AddDecorator(IItem decorator)
    {
        // Do nothing
        return;
    }

    public void AddItem(IItem item) => _items.Add(item.Name, item);

    public IItem RemoveItem(string itemName)
    {
        _items.Remove(itemName, out var itemToReturn);
        return itemToReturn;
    }
}