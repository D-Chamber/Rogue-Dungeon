namespace StarterGame;

public class Item : IItem
{
    private string _name;
    private float _weight;
    private float _value;
    private IItem _decorator;
    private bool _isDecorator;
    
    public Item() : this("NoName") { }

    public Item(string name) : this(name, 1f) { }
    
    public Item(string name, float weight) : this(name, weight, 1f) { }
    
    public Item(string name, float weight, float value) : this(name, weight, value, false)
    {
    }

    public Item(string name, float weight, float value, bool isDecorator)
    {
        _name = name;
        _weight = weight;
        _value = value;
        _decorator = null;
        _isDecorator = isDecorator;
    }
    
    public string Name
    {
        get { return _name; }
    }

    public string LongName
    {
        get
        {
            return Name + (_decorator != null ? ", " + _decorator.LongName : "");
        }
    }
    
    public float Weight
    {
        get { return _weight + (_decorator != null ? _decorator.Weight : 0); }
    }
    
    public float Value
    {
        get { return _value + (_decorator != null ? _decorator.Value : 0); }
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
    
    public string Description { get { return LongName + ":\nweight: " + Weight + ", value: " + Value; } }
}