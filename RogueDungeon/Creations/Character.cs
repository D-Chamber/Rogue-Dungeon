using System.ComponentModel;
using RogueDungeon.Interfaces;

namespace RogueDungeon.Creations;

public class Character : ICharacter
{
    private Room _currentRoom;

    public Room CurrentRoom
    {
        get => _currentRoom;
        private set => _currentRoom = value;
    }

    public IItemContainer _itemContainer { get; set; }
    public string Name { get; set; }
    public Dictionary<string, int> Stats { get; set; }
    public float Gold { get; set; }
    public bool IsTrader { get; private set; }

    public Character(string name = "No Name", bool isTrader = false)
    {
        _itemContainer = new Inventory();
        Name = name;
        Stats = new Dictionary<string, int>();
        Gold = 500f;
        IsTrader = isTrader;
        if (!IsTrader)
        {
            SetStat();
        }
    }

    public void SetRoom(Room room)
    {
        CurrentRoom = room;
    }

    private void SetStat()
    {
        Stats.Add("Level", 1);
        Stats.Add("Health", 65);
        Stats.Add("Strength", 15);
        Stats.Add("Defense", 6);
        Stats.Add("Speed", 10);
        Stats.Add("Experience", 0);
    }

    public void Trade()
    {
        Console.WriteLine(Name + " has this to offer: ");
        var items = _itemContainer.Description;
        Console.Write(items);
    }

    // Character Buys from player
    public void Buy(IItem item)
    {
        if (IsTrader)
        {
            Give(item);
        }
        else
        {
            Console.WriteLine("You can't sell to this character");
        }
    }

    // Character Sells to Player
    public IItem Sell(string itemName)
    {
        var item = Take(itemName);
        if (item != null)
        {
            if (IsTrader)
            {
                return item;
            }
            Give(item);
        }
        else
        {
            Console.WriteLine("You can't buy from this character");
        }

        return null;
    }

    public void Give(IItem item)
    {
        _itemContainer.AddItem(item);
    }

    public IItem Take(string itemName)
    {
        return _itemContainer.RemoveItem(itemName);
    }

    public string Dialogue { get; set; }
}