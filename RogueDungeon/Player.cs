using System.Net.Sockets;
using Pastel;
using RogueDungeon.Creations;
using RogueDungeon.Interfaces;
using RogueDungeon.Logic;

namespace RogueDungeon;

public class Player
{
    // Contains the information that the player knows
    private Room _currentRoom;
    private readonly IItemContainer _inventory;
    private Stack<Room> _roomHistory;

    // creates a Dictionary of equipped items
    private readonly Dictionary<string, IItem> _equippedItems;
    
    // private variables for stats (if needed)
    private int _health;
    private float _speed;
    private float _normalSpeed;

    // Properties
    public Room CurrentRoom
    {
        get
        {
            return _currentRoom;
        }
        private set => _currentRoom = value;
    }

    public int Level { get; set; }

    public int MaxHealth
    {
        get
        {
            // Takes the initial health and have it as a constant until you level up.
            var maxHealth = 50;
            // Adds 5 health for every level up.
            maxHealth += (Level == 1 ? 0 : Level * 5);
            return maxHealth;
        }
    }

    public int Health
    {
        get => _health;
        set => _health = value;
    }
    public int Strength { get; set; }
    public int Defense { get; set; }

    public float Speed
    {
        get
        {
            _equippedItems.TryGetValue("Armor", out var item);
            var armor = item as Armor;
            return _speed *= (armor == null ? 1 : armor.SpeedMod);
        }
        set => _speed = value;
    }

    public int Experience { get; set; }
    public float Gold { get; set; }

    public float CurrentWeight
    {
        get;
        set;
    }

    public float MaxWeight
    {
        get
        {
            return Strength * 6;
        }
    }

    private readonly Dictionary<string, State> _states = new Dictionary<string, State>()
    {
        ["normal"] = new RunningState(),
        ["shop"] = new ShoppingState(),
        ["fight"] = new FightingState(),
        ["dead"] = new DeadState(),
        ["win"] = new WinState()
    };

    private readonly PlayerState playerState;

    // Constructor
    public Player(Room startingRoom)
    {
        CurrentRoom = startingRoom;
        _inventory = new Inventory();
        _roomHistory = new Stack<Room>();
        Level = 1;
        Health = 50;
        Strength = 10;
        Defense = 10;
        Speed = 10f;
        Experience = 0;
        Gold = 40f;
        CurrentWeight = 0;
        _equippedItems = new Dictionary<string, IItem>();
        playerState = new PlayerState(_states["normal"]);
        playerState.Update(this);

        NotificationCenter.Instance.AddObserver("PlayerFight", Fight);
    }

    // Base Behaviors
    public void WalkTo(string direction)
    {
        var door = this.CurrentRoom.GetExit(direction);
        if (door != null)
        {
            if (door.IsOpen)
            {
                var nextRoom = door.RoomOnTheOtherSide(CurrentRoom);
                _roomHistory.Push(CurrentRoom);
                CurrentRoom = nextRoom;
                NotificationCenter.Instance.PostNotification(new Notification("PlayerEnteredRoom", this));
                OutputMessage("\n" + CurrentRoom.Description().Pastel("#F7F6CF"));
            }
            else
            {
                OutputMessage("\nThe door on the " + direction + " is closed.");
            }
        }
        else
        {
            OutputMessage("\nThere is no door on the " + direction + ".");
        }
    }

    public void OutputMessage(string message)
    {
        Console.WriteLine(message);
    }

    // State management methods used in tandem with the PlayerState context manager.
    public void ExitState()
    {
        NotificationCenter.Instance.PostNotification(new Notification("LeavingState", this));
        OutputMessage("You have left.");
        GoBack();
    }
    
    public void EnterWinningState()
    {
        playerState.TransitionTo(_states["win"]);
        playerState.Update(this);
    }


    /*
     * Command Behaviors:
     * These are the methods that influences what the player CAN do especially with commands in the game.
     */
    public void Add(IConsumable decorator)
    {
        OutputMessage("\nWhat item do you want to add the " + decorator.Name + " to?");
        Console.Write("\n>");
        var input = Console.ReadLine();
        var item = Take(input);
        if (item != null)
        {
            item.AddDecorator(decorator);
            OutputMessage("\nYou added the " + decorator.Name + " to the " + item.Name + ".");
            Give(item);
        }
        else
        {
            Give(decorator);
            OutputMessage("\nYou don't have a " + input + " in your inventory.");
        }
    }
    
    public void Close(string doorName)
    {
        var door = CurrentRoom.GetExit(doorName);
        if (door != null)
        {
            if (door.IsClosed)
            {
                OutputMessage("\nThe door on " + doorName + " is already closed.");
            }
            else
            {
                if (door.Close())
                {
                    OutputMessage("\nThe door on " + doorName + " is now closed.");
                }
                else
                {
                    OutputMessage("\nThe door on " + doorName + " cannot be closed.");
                }
            }
        }
        else
        {
            OutputMessage("\nThere is no door on " + doorName);
        }
    }
    
    private void DisplayEquipped()
    {
        OutputMessage("\nEquipped Items:");
        foreach (var item in _equippedItems)
        {
            OutputMessage("\n" + item.Value.Name);
        }
    }
    
    public void DisplayInventory()
    {
        OutputMessage($"\n{_inventory.Description}");
        if (_equippedItems.Count > 0)
        {
            
            DisplayEquipped();
        }
    }
        
    public void DropItem(string itemName)
    {
        var item = Take(itemName);
        if (item != null)
        {
            CurrentRoom.PlaceItem(item);
            CurrentWeight -= item.Weight;
            OutputMessage("\nYou dropped the " + itemName + ".");
        }
        else
        {
            OutputMessage("\nYou don't have a " + itemName + " in your inventory.");
        }
    }
    
    public void Equip(string itemName)
    {
        var item = _inventory.RemoveItem(itemName);
        if (item != null && !_equippedItems.ContainsValue(item))
        {
            if (item is IWeapon)
            {
                _equippedItems["Weapon"] = item;
                OutputMessage("\nYou equipped the " + itemName + ".");
                _inventory.AddItem(item);
            }
            else if (item is IArmor)
            {
                _normalSpeed = _speed;
                _equippedItems["Armor"] = item;
                OutputMessage("\nYou equipped the " + itemName + ".");
                _inventory.AddItem(item);
            }
            else
            {
                OutputMessage("\nYou cannot equip a " + itemName + ".");
                _inventory.AddItem(item);
            }
        }
        else if (item != null && _equippedItems.ContainsValue(item))
        {
            if (item is IWeapon)
            {
                UnEquip("Weapon", item);
                OutputMessage($"\nYou un-equipped the {item.Name}.");
            }
            else if (item is IArmor)
            {
                Speed = _normalSpeed;
                UnEquip("Armor", item);
                OutputMessage($"\nYou un-equipped the {item.Name}.");
            }
        }
        else
        {
            OutputMessage("\nYou don't have a " + itemName + " in your inventory.");
        }
    }
    
    public void Give(IItem item)
    {
        _inventory.AddItem(item);
    }

    public void GoBack()
    {
        if (_roomHistory.Count > 0)
        {
            _roomHistory.TryPop(out _currentRoom);
            OutputMessage("\nYou went back.");
            OutputMessage("\n" + CurrentRoom.Description().Pastel("#F7F6CF"));
        }
        else
        {
            OutputMessage("\nYou can't go back any further.");
        }
    }

    public void Inspect(string itemName)
    {
        // Check if the item is in the player's inventory
        var item = _inventory.RemoveItem(itemName);
        if (item != null)
        {
            OutputMessage(item.Description);
            _inventory.AddItem(item);
        }
        else
        {
            item = CurrentRoom.PickupItem(itemName);
            if (item != null)
            {
                OutputMessage("\n" + item.Description);
                CurrentRoom.PlaceItem(item);
            }
            else
            {
                OutputMessage("\nThere is no " + itemName + " in the room.");
            }
        }
    }

    public void Open(string doorName)
    {
        var door = CurrentRoom.GetExit(doorName);
        if (door != null)
        {
            if (door.IsOpen)
            {
                OutputMessage("\nThe door on " + doorName + " is already open.");
            }
            else
            {
                if (door.Open())
                {
                    OutputMessage("\nThe door on " + doorName + " is now open.");
                }
                else
                {
                    OutputMessage("\nThe door on " + doorName + " cannot be open.");
                }
            }
        }
        else
        {
            OutputMessage("\nThere is no door on " + doorName);
        }
    }

    public void Pickup(string itemName)
    {
        var item = CurrentRoom.PickupItem(itemName);
        if (item != null && CurrentWeight + item.Weight <= MaxWeight)
        {
            Give(item);
            CurrentWeight += item.Weight;
            OutputMessage("\nYou picked up the " + itemName + ".");
        }
        else if (item != null && CurrentWeight + item.Weight > MaxWeight)
        {
            CurrentRoom.PlaceItem(item);
            OutputMessage("\nYou can't carry that much weight.");
        }
        else
        {
            OutputMessage("\nThere is no " + itemName + " in the room.");
        }
    }

    public void Say(string word)
    {
        OutputMessage("\nYou said: " + word);
        Dictionary<string, object> userInfo = new Dictionary<string, object>();
        userInfo["word"] = word;
        var notification = new Notification("PlayerSaidWord", this, userInfo);
        NotificationCenter.Instance.PostNotification(notification);
    }
    
    public void ShowStatus()
    {
        OutputMessage("______________________________");
        OutputMessage("\n| Level: " + Level);
        OutputMessage("| Health: " + Health + "/" + MaxHealth);
        OutputMessage("| Attack: " + Strength);
        OutputMessage("| Defense: " + Defense);
        OutputMessage("| Speed: " + Speed);
        OutputMessage("| Experience: " + Experience);
        OutputMessage("| Gold: " + Gold);
        OutputMessage("| Weight: " + CurrentWeight + "/" + MaxWeight);
        OutputMessage("______________________________");
    }

    public IItem Take(string itemName)
    {
        return _inventory.RemoveItem(itemName);
    }
    
    public void Talk()
    {
        OutputMessage("\nWho do you wanna talk to?");
        Console.Write("\n>");
        var characterName = Console.ReadLine();
        OutputMessage("\n");
        var character = CurrentRoom.GetCharacter(characterName);
        if (character == null) 
            OutputMessage("\nThere is no " + characterName + " in the room.");
        else
        {
            if (character.IsTrader)
            {
                playerState.TransitionTo(_states["shop"]);
                playerState.Update(this);
                character.Trade();
            }
            
            if (character.Name == "Guard")
            {
                var item = character.Take("Key");
                if (item != null)
                {
                    OutputMessage(character.Dialogue);
                    Give(item);
                }
                else if (item == null)
                {
                    OutputMessage("\nThe guard doesn't have a key.");
                }
                else
                {
                    character.Give(item);
                }
            }
            
            // same with the pharmacist as the guard.
            else if (characterName == "Pharmacist")
            {
                var item = character.Take("Potion");
                if (item != null)
                {
                    OutputMessage(character.Dialogue);
                    Give(item);
                }
                else if (item == null)
                {
                    OutputMessage("\nThe pharmacist doesn't have any more potions.");
                }
                else
                {
                    character.Give(item);
                }
            }
            
            else
            {
                OutputMessage(character.Dialogue);
            }
        }
    }

    public void Talk(string characterName)
    {
        var character = CurrentRoom.GetCharacter(characterName);
        if (character == null) 
            OutputMessage("\nThere is no " + characterName + " in the room.");
        else
        {
            if (character.IsTrader)
            {
                playerState.TransitionTo(_states["shop"]);
                playerState.Update(this);
                character.Trade();
            }
            
            // if you talk to Guard this is what he says cause he has a special condition
            if (character.Name == "Guard")
            {
                var item = character.Take("Key");
                if (item != null)
                {
                    OutputMessage(character.Dialogue);
                    Give(item);
                }
                else if (item == null)
                {
                    OutputMessage("\nThe guard doesn't have a key.");
                }
                else
                {
                    character.Give(item);
                }
            }

            // same with the pharmacist as the guard.
            else if (characterName == "Pharmacist")
            {
                var item = character.Take("Potion");
                if (item != null)
                {
                    OutputMessage(character.Dialogue);
                    Give(item);
                }
                else if (item == null)
                {
                    OutputMessage("\nThe pharmacist doesn't have any more potions.");
                }
                else
                {
                    character.Give(item);
                }
            }

            // everyone else you talk to will just say their dialogue
            else
            {
                OutputMessage(character.Dialogue);
            }
        }
    }
    
    public void UnEquip(string itemType, IItem item)
    {
        _inventory.AddItem(item);
        _equippedItems.Remove(itemType);
    }

    /*
     * Shopping System
     */
    public void Buy()
    {
        OutputMessage("\nWho do you wanna buy from?");
        Console.Write("\n>");
        var characterName = Console.ReadLine();
        OutputMessage("\n");
        var otherCharacter = CurrentRoom.GetCharacter(characterName);
        if (otherCharacter == null) 
            OutputMessage("\nThere is no " + characterName + " in the room.");
        else
        {
            Talk();
            // otherCharacter.Trade();
        }
    }
    
    public void Buy(string itemName)
    {
        OutputMessage("\nWho do you wanna talk to?");
        Console.Write("\n>");
        var characterName = Console.ReadLine();
        OutputMessage("\n");
        var character = CurrentRoom.GetCharacter(characterName);
        if (character != null)
        {
            var item = character.Sell(itemName);
            if (item != null)
            {
                if (Gold >= item.Value)
                {
                    if (CurrentWeight + item.Weight <= MaxWeight)
                    {
                        _inventory.AddItem(item);
                        Gold -= item.Value;
                        CurrentWeight += item.Weight;
                        OutputMessage("\nYou bought the " + item.Name + " for " + item.Value + " gold.");
                    }
                    else
                    {
                        OutputMessage("\nYou can't carry any more items.");
                    }
                }
                else
                {
                    OutputMessage("\nYou don't have enough gold.");
                }
            }
            else
            {
                OutputMessage("\nThere is no " + itemName + " for sale.");
            }
        }
        else
        {
            OutputMessage("\nThere is no " + characterName + " in the room.");
        }
    }
    
    public void Sell(string itemName)
    {
        var item = Take(itemName);
        if (item != null)
        {
            var character = CurrentRoom.GetCharacter("Merchant");
            if (character != null)
            {
                Gold += item.Value;
                character.Buy(item);
            }
            else
            {
                Give(item);
                OutputMessage("\nThere is no merchant in the room.");
            }
        }
        else
        {
            OutputMessage("\nYou don't have a " + itemName + " in your inventory.");
        }
    }

    // this is the method that is called to trade items between characters
    public void Trade(string characterName)
    {
        playerState.TransitionTo(_states["shop"]);
        playerState.Update(this);
        var otherCharacter = CurrentRoom.GetCharacter(characterName);
        if (otherCharacter == null) 
            OutputMessage("\nThere is no " + characterName + " in the room.");
        else
        {
            otherCharacter.Trade();
        }
    }
    
    public void Trade(string item1, string item2)
    {
        var character = CurrentRoom.GetCharacter("Merchant");
        var playerItem = _inventory.RemoveItem(item1);
        var otherItem = character?._itemContainer.RemoveItem(item2);
        
        if (playerItem != null && otherItem != null)
        {
            if (playerItem.Value >= otherItem.Value)
            {
                _inventory.AddItem(otherItem);
                character?._itemContainer.AddItem(playerItem);
                OutputMessage($"\nYou traded the {playerItem.Name} for the {otherItem.Name}.");
            }
            else
            {
                character._itemContainer.AddItem(otherItem);
                OutputMessage("\nYou can't trade for that it's not worth it. \nCome back with something better.");
            }
        }
        else if (playerItem == null)
        {
            OutputMessage($"\nYou don't have a {item1} in your inventory.");
        }
        else if (otherItem == null)
        {
            _inventory.AddItem(playerItem);
            OutputMessage($"\nThe merchant doesn't have a {item2} in his inventory.");
        }
        
    }
    
    /*
     * These methods are gonna be used in the Combat System
     */

    public void Fight(Notification notification)
    {
        var character = notification.Object as Character;
        if (character != null)
        {
            playerState.TransitionTo(_states["fight"]);
            playerState.Update(this);
        }
    }

    public void DisplayFight()
    {
        var boss = CurrentRoom.GetCharacter("Boss");
        if (boss != null)
        {
            OutputMessage("\nYou are now fighting the boss.");
            OutputMessage(("\\_______________________/" +
                          "\n| HP: " + Health + "/" + MaxHealth +
                          "\n| Enemy HP: " + boss.Stats["Health"] +
                          "\n/-----------------------\\").Pastel("#e6bd57"));
        }
    }

    public void Attack()
    {
        var rand = new Random();
        var boss = CurrentRoom.GetCharacter("Boss");
        if (boss != null)
        {
            _equippedItems.TryGetValue("Weapon", out var item);
            var weapon = item as IWeapon;
            var damage = Strength + (weapon == null ? 0 : rand.Next(weapon.DamageMod[0], weapon.DamageMod[1] + 1));
            var bossMove = rand.Next(1, 3);
            
            switch (bossMove)
            {
                case 1:
                    OutputMessage("You attack the boss.".Pastel("#f05457"));
                    boss.Stats["Health"] -= damage - boss.Stats["Defense"];
                    OutputMessage($"The {boss.Name} attacks you.".Pastel("#f05457"));
                    Health -= (boss.Stats["Strength"] - Defense < 0) ? 0 : boss.Stats["Strength"] - Defense;
                    break;
                case 2:
                    OutputMessage("You attack the boss.".Pastel("#f05457"));
                    OutputMessage("The boss defends against your attack.".Pastel("#f05457"));
                    boss.Stats["Health"] -= (damage - boss.Stats["Defense"] < 0) ? 0 : Strength - boss.Stats["Defense"];
                    break;
            }
            
            if (Health <= 0)
            {
                OutputMessage("\nYou died.".Pastel("#f05457"));
                Health = 0;
                playerState.TransitionTo(_states["dead"]);
                playerState.Update(this);
            }
            else if (boss.Stats["Health"] <= 0)
            {
                OutputMessage("\nYou killed the boss.".Pastel("#f05457"));
                boss.Stats["Health"] = 0;
                playerState.TransitionTo(_states["normal"]);
                playerState.Update(this);
                CurrentRoom.RemoveCharacter(boss);
                DisplayFight();
            }
        }
        DisplayFight();
    }

    public void Defend()
    {
        var rand = new Random();
        var boss = CurrentRoom.GetCharacter("Boss");
        if (boss != null)
        {
            _equippedItems.TryGetValue("Armor", out var item);
            var armor = item as IArmor;
            var mitigation = Defense + (armor == null ? 0 : rand.Next(armor.ArmorMod[0], armor.ArmorMod[1] + 1));
            var bossMove = rand.Next(1, 3);
            switch (bossMove)
            {
                case 1:
                    OutputMessage("You defend against the boss's attack.".Pastel("#f05457"));
                    Health -= (boss.Stats["Strength"] - mitigation < 0) ? 0 : boss.Stats["Strength"] - mitigation;
                    OutputMessage($"The {boss.Name} attacks you.".Pastel("#f05457"));
                    break;
                case 2:
                    OutputMessage("You defend against the boss's attack.".Pastel("#f05457"));
                    OutputMessage("The boss defends against your attack.".Pastel("#f05457"));
                    break;
            }
            if (Health <= 0)
            {
                OutputMessage("\nYou died.");
                playerState.TransitionTo(_states["dead"]);
                playerState.Update(this);
            }
            else if (boss.Stats["Health"] <= 0)
            {
                OutputMessage("\nYou killed the boss.");
                playerState.TransitionTo(_states["normal"]);
                playerState.Update(this);
            }
        }

        DisplayFight();
    }

    public void Use(string itemName)
    {
        var rand = new Random();
        var item = _inventory.RemoveItem(itemName);
        if (item != null && item is IConsumable)
        {
            var consumable = item as IConsumable;
            if (consumable.Name == "Potion")
            {
                if (consumable.Count > 0)
                {
                    var potion = new int[] { consumable.Modifier[0], consumable.Modifier[1] };
                    var healAmount = rand.Next(potion[0], potion[1]);
                    if (Health == MaxHealth)
                    {
                        Give(consumable);
                        OutputMessage("\nYou are already at full health.");
                    }
                    else if (Health + healAmount > MaxHealth)
                    {
                        consumable.Count--;
                        if (consumable.Count > 0)
                        {
                            Give(consumable);
                        }
                        OutputMessage($"\nYou used a {consumable.Name} and healed {healAmount} health.");
                        Health = MaxHealth;
                    }
                    else
                    {
                        consumable.Count--;
                        if (consumable.Count > 0)
                        {
                            Give(consumable);
                        }
                        OutputMessage($"\nYou used a {consumable.Name} and healed {healAmount} health.");
                        Health += healAmount;
                    }
                }
            }
            
            if (consumable.IsDecorator)
            {
                Add(consumable);
            }
        }
        else
        {
            OutputMessage("\nYou don't have a " + itemName + " in your inventory.");
        }
    }

    public void Use(string itemName, string exitName)
    {
        var item = Take(itemName);
        var door = CurrentRoom.GetExit(exitName);

        if (item != null && door != null)
        {
            if (item is IConsumable)
            {
                var key = item as IConsumable;
                if (item.Name == "Key")
                {
                    OutputMessage("\nYou unlocked the door.");
                    door.TheLock.Unlock();
                }
            }
            else
            {
                OutputMessage("\nYou can't use that here.");
            }
        }
        else
        {
            OutputMessage("\nYou don't have a " + itemName + " in your inventory.");
        }

    }
}