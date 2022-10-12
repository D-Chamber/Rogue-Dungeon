using System;

namespace StarterGame
{
     public class GameWorld
     {
          public Room CreateWorld()
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

            outside.SetExit("west", boulevard);

            boulevard.SetExit("east", outside);
            boulevard.SetExit("south", scctparking);
            boulevard.SetExit("west", theGreen);
            boulevard.SetExit("north", universityParking);

            scctparking.SetExit("west", scct);
            scctparking.SetExit("north", boulevard);

            scct.SetExit("east", scctparking);
            scct.SetExit("north", schuster);

            schuster.SetExit("south", scct);
            schuster.SetExit("north", universityHall);
            schuster.SetExit("east", theGreen);

            theGreen.SetExit("west", schuster);
            theGreen.SetExit("east", boulevard);

            universityHall.SetExit("south", schuster);
            universityHall.SetExit("east", universityParking);

            universityParking.SetExit("south", boulevard);
            universityParking.SetExit("west", universityHall);
            universityParking.SetExit("north", parkingDeck);

            parkingDeck.SetExit("south", universityParking);

            return outside;
        }
     }
}