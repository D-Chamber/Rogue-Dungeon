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

          private int counter;

          private Dictionary<Room, IWorldEvent> worldEvents = new Dictionary<Room, IWorldEvent>();
          // private WorldMod worldMod;

          private GameWorld()
          {
              CreateWorld();
              NotificationCenter.Instance.AddObserver("PlayerDidEnterRoom", PlayerDidEnterRoom);
              NotificationCenter.Instance.AddObserver("PlayerWillEnterRoom", PlayerWillEnterRoom);
              counter = 0;
          }

          public void PlayerDidEnterRoom(Notification notification) 
          {
               Player player = (Player)notification.Object;
               if (player != null)
               {
                    if (player.CurrentRoom == Exit)
                    {
                         player.OutputMessage("\n*** The player reached the exit.");
                         counter++;
                         if (counter == 5)
                         {
                              // Exit.SetExit("shortcut", Entrance);
                              // Entrance.SetExit("shortcut", Exit);
                         }
                    }

                    if (player.CurrentRoom == Entrance)
                    {
                         player.OutputMessage("\n*** The player came back to the entrance.");
                    }

                    IWorldEvent worldEvent = null;
                    worldEvents.TryGetValue(player.CurrentRoom, out worldEvent);
                    if (worldEvent != null)
                    {
                         worldEvent.Execute();
                         player.OutputMessage("\n%%% There is a change in the world. %%%\n");
                         RemoveWorldEvent(worldEvent);
                    }
               }
          }

          public void PlayerWillEnterRoom(Notification notification)
          {
               Player player = (Player)notification.Object;
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
               Room outside = new Room("outside the main entrance of the university");
               Room scctparking = new Room("in the parking lot at SCCT");
               Room boulevard = new Room("on the boulevard");
               Room universityParking = new Room("in the parking lot at University Hall");
               Room parkingDeck = new Room("in the parking deck");
               Room scct = new Room("in the SCCT building");
               Room theGreen = new Room("in the green in from of Schuster Center");
               Room universityHall = new Room("in University Hall");
               Room schuster = new Room("in the Schuster Center");

               // Connect the Rooms
               // outside.SetExit("west", boulevard);
               // boulevard.SetExit("east", outside);
               
               // Door door = new Door(boulevard, outside);
               // boulevard.SetExit("east", door);
               // outside.SetExit("west", door);
               Door door = Door.CreateDoor(boulevard, outside, "east", "west");
               
               // boulevard.SetExit("south", scctparking);
               // scctparking.SetExit("north", boulevard);
               door = Door.CreateDoor(boulevard, scctparking, "north", "south");

               // boulevard.SetExit("west", theGreen);
               // theGreen.SetExit("east", boulevard);
               door = Door.CreateDoor(boulevard, theGreen, "west", "east");

               // boulevard.SetExit("north", universityParking);
               // universityParking.SetExit("south", boulevard);
               door = Door.CreateDoor(boulevard, universityHall, "south", "north");
               
               // scctparking.SetExit("west", scct);
               // scct.SetExit("east", scctparking);
               door = Door.CreateDoor(scctparking, scct, "east", "west");

               // scct.SetExit("north", schuster);
               // schuster.SetExit("south", scct);
               door = Door.CreateDoor(scct, schuster, "south", "north");

               // schuster.SetExit("north", universityHall);
               // universityHall.SetExit("south", schuster);
               door = Door.CreateDoor(schuster, universityHall, "south", "north");

               // schuster.SetExit("east", theGreen);
               // theGreen.SetExit("west", schuster);
               door = Door.CreateDoor(schuster, theGreen, "west", "east");

               // universityHall.SetExit("east", universityParking);
               // universityParking.SetExit("west", universityHall);
               door = Door.CreateDoor(universityHall, universityParking, "west", "east");

               // universityParking.SetExit("north", parkingDeck);
               // parkingDeck.SetExit("south", universityParking);
               door = Door.CreateDoor(universityParking, parkingDeck, "south", "north");

               // Extra rooms
               Room davidson = new Room("in the Davidson Center");
               Room clockTower = new Room("at the Clock Tower");
               Room greekCenter = new Room("at the Greek Center");
               Room woodall = new Room("at Woodall Hall");

               // Connect the Davidson to Clock Tower and others
               // davidson.SetExit("west", clockTower);
               // clockTower.SetExit("east", davidson);
               door = Door.CreateDoor(davidson, clockTower, "east", "west");
               
               // clockTower.SetExit("north", greekCenter);
               // greekCenter.SetExit("south", clockTower);
               door = Door.CreateDoor(clockTower, greekCenter, "south", "north");

               // clockTower.SetExit("south", woodall);
               // woodall.SetExit("north", clockTower);
               door = Door.CreateDoor(clockTower, woodall, "north", "south");

               // Setup connection
               IWorldEvent worldMod = new WorldMod(parkingDeck, schuster, davidson, "east", "west");
               AddWorldEvent(worldMod);

               // Create Lumpkin Center and Recreation Center
               Room lumpkin = new Room("in the Lumpkin Center");
               Room recreation = new Room("in the Recreation Center");

               // Connect Lumpkin Center to Recreation Center
               // lumpkin.SetExit("west", recreation);
               // recreation.SetExit("east", lumpkin);
               door = Door.CreateDoor(lumpkin, recreation, "east", "west");

               // Setup connection
               worldMod = new WorldMod(scct, parkingDeck, lumpkin, "south", "north");
               AddWorldEvent(worldMod);

               worldMod = new WorldMod(woodall, recreation, greekCenter, "east", "west");
               AddWorldEvent(worldMod);

               // Setup rooms with delegates
               IRoomDelegate trapRoom = new TrapRoom();
               scct.Delegate = trapRoom;
               // trapRoom.ContainingRoom = scct;

               IRoomDelegate echoRoom = new EchoRoom();
               parkingDeck.Delegate = echoRoom;
               // echoRoom.ContainingRoom = parkingDeck;

               // assign special rooms
               _entrance = outside;
               _exit = schuster;
          }
     }
}