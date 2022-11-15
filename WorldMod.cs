using System;
using System.Collections.Generic;
using System.Text;

namespace StarterGame
{
     public class WorldMod : IWorldEvent
     {
          private Room _trigger;
          private Room _sideA;
          private Room _sideB;
          private string _toSideA;
          private string _toSideB;

          public Room Trigger { get { return _trigger; } }

          public WorldMod(Room trigger, Room sideA, Room sideB, string toSideA, string toSideB)
          {
               _trigger = trigger;
               _sideA = sideA;
               _sideB = sideB;
               _toSideA = toSideA;
               _toSideB = toSideB;
          }

          public void Execute()
          {
               // _sideA.SetExit(_toSideB, _sideB);
               // _sideB.SetExit(_toSideA, _sideA);
               var door = Door.CreateDoor(_sideA, _sideB, _toSideA, _toSideB);
               door.Close();
          }
     }
}