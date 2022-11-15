using System;
using System.Collections.Generic;
using System.Text;

namespace StarterGame
{
     public class GameWorld
     {
          private static GameWorld _instance = null;
          public static GameWorld Instance
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

          private Room _entrance;
          public Room Entrance { get { return _entrance; } }

          private Room _exit;
          public Room Exit { get { return _exit; } }

          private int _counter;

          private Dictionary<Room, IWorldEvent> worldEvents = new Dictionary<Room, IWorldEvent>();
          // private WorldMod worldMod;

          private GameWorld()
          {
              CreateWorld();
              NotificationCenter.Instance.AddObserver("PlayerDidEnterRoom", PlayerDidEnterRoom);
              NotificationCenter.Instance.AddObserver("PlayerWillEnterRoom", PlayerWillEnterRoom);
              _counter = 0;
          }

          private void PlayerDidEnterRoom(Notification notification) 
          {
               var player = (Player)notification.Object;
               if (player != null)
               {
                    if (player.CurrentRoom == Exit)
                    {
                         player.OutputMessage("\n*** The player reached the exit.");
                         _counter++;
                         if (_counter == 5)
                         {
                              // Exit.SetExit("shortcut", Entrance);
                              // Entrance.SetExit("shortcut", Exit);
                         }
                    }

                    if (player.CurrentRoom == Entrance)
                    {
                         player.OutputMessage("\n*** The player came back to the entrance.");
                    }

                    worldEvents.TryGetValue(player.CurrentRoom, out var worldEvent);
                    if (worldEvent != null)
                    {
                         worldEvent.Execute();
                         player.OutputMessage("\n%%% There is a change in the world. %%%\n");
                         RemoveWorldEvent(worldEvent);
                    }
               }
          }

          private void PlayerWillEnterRoom(Notification notification)
          {
               var player = (Player)notification.Object;
               if (player != null)
               {
                    if (player.CurrentRoom == Entrance)
                    {
                         player.OutputMessage("\n>>> The player is leaving the entrance.");
                    }
                    if (player.CurrentRoom == Exit)
                    {
                         player.OutputMessage("\n>>> The player is going away from the exit.");
                    }
               }
          }

          private void AddWorldEvent(IWorldEvent worldEvent)
          {
               worldEvents[worldEvent.Trigger] = worldEvent;
          }

          private void RemoveWorldEvent(IWorldEvent worldEvent)
          {
               worldEvents.Remove(worldEvent.Trigger);
          }

          private void CreateWorld()
          {
               var outside = new Room("outside the main entrance of the university");
               var scctparking = new Room("in the parking lot at SCCT");
               var boulevard = new Room("on the boulevard");
               var universityParking = new Room("in the parking lot at University Hall");
               var parkingDeck = new Room("in the parking deck");
               var scct = new Room("in the SCCT building");
               var theGreen = new Room("in the green in front of Schuster Center");
               var universityHall = new Room("in University Hall");
               var schuster = new Room("in the Schuster Center");

               // Connect the Rooms
               Door door = Door.CreateDoor(boulevard, outside, "east", "west");
               
               door = Door.CreateDoor(boulevard, scctparking, "south", "north");
               
               door = Door.CreateDoor(boulevard, theGreen, "west", "east");
               
               door = Door.CreateDoor(boulevard, universityParking, "north", "south");
               
               door = Door.CreateDoor(scctparking, scct, "west", "east");


               door = Door.CreateDoor(scct, schuster, "north", "south");
               door.Close();
               ILockable regularLock = new RegularLock();
               door.TheLock = regularLock;
               regularLock.Lock();
               
               door = Door.CreateDoor(schuster, universityHall, "north", "south");
               
               door = Door.CreateDoor(schuster, theGreen, "east", "west");

               door = Door.CreateDoor(
                    universityHall, universityParking, "east", "west"
               );

               door = Door.CreateDoor(
                    universityParking, parkingDeck, "north", "south"
               );

               // Extra rooms
               var davidson = new Room("in the Davidson Center");
               var clockTower = new Room("at the Clock Tower");
               var greekCenter = new Room("at the Greek Center");
               var woodall = new Room("at Woodall Hall");

               // Connect the Davidson to Clock Tower and others
               door = Door.CreateDoor(davidson, clockTower, "west", "east");
               
               door = Door.CreateDoor(clockTower, greekCenter, "north", "south");

               door = Door.CreateDoor(clockTower, woodall, "south", "north");

               // Setup connection
               IWorldEvent worldMod = new WorldMod(
                    parkingDeck, schuster, davidson, "west", "east"
                    );
               AddWorldEvent(worldMod);

               // Create Lumpkin Center and Recreation Center
               var lumpkin = new Room("in the Lumpkin Center");
               var recreation = new Room("in the Recreation Center");

               // Connect Lumpkin Center to Recreation Center
               door = Door.CreateDoor(lumpkin, recreation, "west", "east");

               // Setup connection
               worldMod = new WorldMod(
                    scct, parkingDeck, lumpkin, "north", "south"
                    );
               AddWorldEvent(worldMod);

               worldMod = new WorldMod(
                    woodall, recreation, greekCenter, "west", "east"
                    );
               AddWorldEvent(worldMod);

               // Setup rooms with delegates
               IRoomDelegate trapRoom = new TrapRoom();
               scct.Delegate = trapRoom;

               IRoomDelegate echoRoom = new EchoRoom();
               parkingDeck.Delegate = echoRoom;
               
               // Create and drop items in rooms
               IItem item = new Item("Dagger", 1.5f, 2f);
               IItem decorator = new Item("Ruby", 0.5f, 10f, true);
               item.AddDecorator(decorator);
               
               decorator = new Item("Emerald", 0.5f, 10f, true);

               item.AddDecorator(decorator);
               boulevard.Drop(item);

               // assign special rooms
               _entrance = outside;
               _exit = schuster;
          }
     }
}