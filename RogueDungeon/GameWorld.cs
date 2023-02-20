using System.ComponentModel;
using Pastel;
using RogueDungeon.Creations;
using RogueDungeon.Interfaces;
using RogueDungeon.Logic;

namespace RogueDungeon;

public class GameWorld
{
    private static GameWorld? _instance;

    public static GameWorld? Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameWorld();
            }

            return _instance;
        }
    }

    public Room Entrance
    {
        get;
        private set;
    }

    public Room Exit
    {
        get;
        private set;
    }
    
    public Room BossRoom
    {
        get;
        private set;
    }

    private Dictionary<Room, IWorldEvent> worldEvents = new Dictionary<Room, IWorldEvent>();
    private Dictionary<string, IItem> items = new Dictionary<string, IItem>();

    // Constructor
    private GameWorld()
    {
        CreateItems();
        CreateWorld();
        // GameWorld listens to the player if they walked into a room and do checks.
        NotificationCenter.Instance.AddObserver("PlayerEnteredRoom", PlayerEnterRoom);
    }

    // The method implemented by the observer
    private void PlayerEnterRoom(Notification notification)
    {
        var player = (Player)notification.Object;
        if (player != null)
        {
            if (player.CurrentRoom == Exit)
            {
                player.OutputMessage("\nYou have escaped the dungeon!");
                NotificationCenter.Instance.PostNotification(new Notification("Finished"));
            }
      
            if (player.CurrentRoom == Entrance)
            {
                player.OutputMessage("\nYou are at the entrance to the dungeon.");
            }

            if (player.CurrentRoom.GetCharacter("Boss") != null)
            {
                var boss = (Character) player.CurrentRoom.GetCharacter("Boss");
                player.OutputMessage("\nYou have encountered the boss!");
                NotificationCenter.Instance.PostNotification(new Notification("PlayerFight", boss));
            }

            worldEvents.TryGetValue(player.CurrentRoom, out var worldEvent);
            if (worldEvent != null)
            {
                worldEvent.Execute();
                player.OutputMessage("\n%%% There was a change in the world. %%%");
                RemoveWorldEvent(worldEvent);
            }  
        }
    }

    // used to add worldevents but not used at the moment
    private void AddWorldEvent(IWorldEvent worldEvent)
    {
        worldEvents[worldEvent.Trigger] = worldEvent;
    }
    
    public void RemoveWorldEvent(IWorldEvent worldEvent)
    {
        worldEvents.Remove(worldEvent.Trigger);
    }
    
    /*
     * Creates the items to put in the game
     */
    private void CreateItems()
    {
        var itemsList = new List<IItem>()
        {
            new Armor("Buckler", 5f, 10f)
            {
                ArmorMod = new[] { 5, 7 }
            },
            new Armor("Chainmail", 10f, 20f)
            {
                ArmorMod = new[] { 10, 15 }
            },
            new Armor("Plate Armor", 15f, 30f)
            {
                ArmorMod = new[] { 15, 20 }
            },
            new Weapon("Dagger", 2f, 10f)
            {
                DamageMod = new[] { 3, 6 }
            },
            new Weapon("Sword", 4f, 10f)
            {
                DamageMod = new[] { 5, 7 }
            },
            new Weapon("Greatsword", 6f, 10f)
            {
                DamageMod = new[] { 7, 10 }
            },
            new Weapon("Battle Axe", 6f , 8f)
            {
                DamageMod = new[] { 8, 12 }
            },
            new Consumable("Potion", 1f, 2f, 5)
            {
                Modifier = new[] { 5, 10 }
            },
            new Consumable("Key", 1f, 1f, 1, true),
            new Consumable("Ruby", .5f, 4f, 1, true)
            {
                Modifier = new[] { 2, 5 }
            },
            new Consumable("Diamond", 1f, 7f, 1, true)
            {
                Modifier = new[] { 5, 8 }
            },
            new Consumable("Emerald", 1f, 5f, 1, true)
            {
                Modifier = new[] { 3, 6 }
            },
        };
        
        foreach (var item in itemsList)
        {
            items[item.Name] = item;
        }
    }

    /* 
     * Create the world and all the rooms.
     * Adds the Connections and also Adds the
     * Items and Creatures to the rooms.
     */
    private void CreateWorld()
    {
        Door door;
        
        // Creates the Characters
        var merchant = new Character("Merchant", true);
        var guard = new Character("Guard")
        {
            Dialogue = ("\nYou have to help me the king is in danger! I heard a scream from the throne room. " +
                       "\nI think the king is in trouble." + " Please ".Pastel("#f05457") + "help me! Here take this key. It should help you get into the room." +
                       "\nDon't worry I will be fine. I will wait for you here." +
                       "\n\n -- The guard hands you a key --").Pastel("#c7a29c")
        };
        var pharmacist = new Character("Pharmacist")
        {
            Dialogue = ("\nHello there! I am the pharmacist. I can help you with your health. " +
                       "\nI have some potions that will help you. " +
                       "\n\n -- The pharmacist hands you a potion --").Pastel("#c7a29c")
        };
        
        var boss = new Character("Boss");
        var king = new Character("King");

        // Creates the Rooms
        var outside = new Room("outside the entrance of the gates");
        var hallways = new Room("in the hallways");
        var apothecary = new Room("in the apothecary");
        var armory = new Room("in the armory");
        var throneRoom = new Room("in the throne room");
        var forge = new Room("in the forge");
        var cave = new Room("in the cave");
        var outsideCave = new Room("outside the cave");
        var garden = new Room("in the garden");
        var bossRoom = new Room("in the boss room");
        var shopRoom = new Room("in the shop room");
        var royalChambers = new Room("in the royal chambers");
        var teleporter = new Room("in the " + "teleport".Pastel("#f05457") + "er room");


        // Creates the Connections between the Rooms
        door = Door.CreateDoor(outside, hallways, "north", "south");

        door = Door.CreateDoor(hallways, apothecary, "east", "west");
        
        door = Door.CreateDoor(hallways, armory, "west", "east");
        
        door = Door.CreateDoor(hallways, throneRoom, "north", "south");
        door.Close();
        ILockable regularLock = new RegularLock();
        door.TheLock = regularLock;
        door.Lock();

        door = Door.CreateDoor(throneRoom, bossRoom, "east", "west");
        
        door = Door.CreateDoor(bossRoom, royalChambers, "north", "south");
        
        door = Door.CreateDoor(forge, armory, "east", "west");
        
        door = Door.CreateDoor(garden, armory, "south", "north");

        door = Door.CreateDoor(garden, outsideCave, "north", "south");

        door = Door.CreateDoor(outsideCave, cave, "north", "south");
        door.Close();
        var passwordLock = new PasswordLock("Please");
        door.TheLock = passwordLock;
        door.Lock();
        
        door = Door.CreateDoor(shopRoom, cave, "south", "north");

        door = Door.CreateDoor(shopRoom, teleporter, "west", "east");

        // Special Variables with the Connections
        var otherDoor = new Door(teleporter, throneRoom);
        IRoomDelegate portalRoom = new PortalRoom(otherDoor);
        teleporter.Delegate = portalRoom;

        // place the Characters in the Rooms
        forge.PlaceCharacter(guard);
        bossRoom.PlaceCharacter(boss);
        shopRoom.PlaceCharacter(merchant);
        royalChambers.PlaceCharacter(king);
        apothecary.PlaceCharacter(pharmacist);

        // adds items to rooms
        apothecary.PlaceItem(items["Buckler"]);
        
        armory.PlaceItem(items["Plate Armor"]);
        armory.PlaceItem(items["Battle Axe"]);
        
        hallways.PlaceItem(items["Emerald"]);

        // setup characters inventory
        merchant.Give(items["Potion"]);
        merchant.Give(items["Ruby"]);
        merchant.Give(items["Diamond"]);
        merchant.Give(items["Dagger"]);
        merchant.Give(items["Sword"]);
        
        guard.Give(items["Key"]);

        pharmacist.Give(items["Potion"]);
        
        // Setup for the game to know where the entrance and exit are
        Entrance = outside;
        Exit = royalChambers;
        BossRoom = bossRoom;
    }
}