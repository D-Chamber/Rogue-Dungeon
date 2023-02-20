using Pastel;
using RogueDungeon.Creations;
using RogueDungeon.Interfaces;
using RogueDungeon.Logic;

namespace RogueDungeon;

public class PortalRoom : IRoomDelegate
{
    public Room ContainingRoom { get; set; }
    private Door _destinationRoom;

    public PortalRoom(Door teleporter)
    {
        _destinationRoom = teleporter;
        NotificationCenter.Instance.AddObserver("PlayerSaidWord", PlayerSaid);
    }
    
    public Door GetExit(string exitName)
    {
        ContainingRoom.Delegate = null;
        var exit = ContainingRoom.GetExit(exitName);
        ContainingRoom.Delegate = this;
        return exit;
    }

    public string GetExits()
    {
        var exits = "";
        if (ContainingRoom.Delegate != null)
        {
            ContainingRoom.Delegate = null;
            exits += ContainingRoom.GetExits();
            ContainingRoom.Delegate = this;
        }
        return exits;
    }

    public string Description()
    {
        string description = "\nTry saying something clever.\n\n";
        ContainingRoom.Delegate = null;
        description += ContainingRoom.Description();
        ContainingRoom.Delegate = this;
        

        return description;
    }
    
    public void PlayerSaid(Notification notification)
    {
        var word = notification.UserInfo["word"] as string;
        if (word == "teleport")
        {
            var player = notification.Object as Player;
            if (player.CurrentRoom == ContainingRoom)
            {
                Door.CreateDoor(_destinationRoom, "portal", "portal");
                player.OutputMessage("A portal appears in the room. Looks like you can step inside." +
                                     "\n ***Try it if you dare***".Pastel("#f05457"));
                ContainingRoom.Description();
            }
            
        }
    }
}

public class Room
{
    private Dictionary<string, Door> _exits;
    private string _tag;
    private IItemContainer _items;
    private IRoomDelegate _roomDelegate;
    private Dictionary<string, ICharacter> _characters;
    
    public string Tag
    {
        get => _tag;
        set => _tag = value;
    }
    
    public IRoomDelegate Delegate
    {
        get => _roomDelegate;
        set
        {
            _roomDelegate = value;
            if (Delegate != null)
            {
                _roomDelegate.ContainingRoom = this;
            }
        } 
    }

    public Room() : this("No Tag")
    {
    }

    public Room(string tag)
    {
        Delegate = null;
        _tag = tag;
        _exits = new Dictionary<string, Door>();
        _characters = new Dictionary<string, ICharacter>();
        _items = new Chest();
    }

    public void SetExit(string exitName, Door door)
    {
        _exits[exitName] = door;
    }

    public Door GetExit(string exitName)
    {
        Door door = null;
        if(Delegate != null)
        {
            door = Delegate.GetExit(exitName);
        }
        else
        {
            _exits.TryGetValue(exitName, out door);
        }

        return door;
    }

    public string GetExits()
    {
        var exitNames = "Exits: ";
        if (Delegate != null)
        {
            exitNames += Delegate.GetExits();
        }
        else
        {
            exitNames = _exits.Aggregate(exitNames, 
                (current, exit) => current + (exit.Key + " "));
        }

        return exitNames;
    }

    public bool LockCheck(ILockable TheLock)
    {
        // checks door for instance of the lock
        foreach (var door in _exits)
        {
            if (door.Value.TheLock == TheLock)
            {
                return true;
            }
        }

        return false;
    }

    public void PlaceCharacter(ICharacter character)
    {
        _characters[character.Name] = character;
        character.SetRoom(this);
    }

    public ICharacter? GetCharacter(string name)
    {
        _characters.TryGetValue(name, out var character);
        return character;
    }

    private string GetCharacters()
    {
        var name = "Characters: | ";
        foreach (var character in _characters.Values)
        {
            name += character.Name + " | ";
        }
        
        return name;
    }
    
    public void RemoveCharacter(ICharacter character)
    {
        _characters.Remove(character.Name);
    }

    public void PlaceItem(IItem item)
    {
        _items.AddItem(item);
    }
    
    public IItem PickupItem(string itemName)
    {
        var item = _items.RemoveItem(itemName);

        return item;
    }

    public string Description()
    {
        return Delegate != null
            ? Delegate.Description()
            : $"You are {this.Tag}.\n *** {this.GetExits()}" +
              $"\n >>> {(_characters.Count > 0 ? GetCharacters() : "Characters: <.>")}" +
              $"\n >>> Item: {_items.Description}";
    }
    
}