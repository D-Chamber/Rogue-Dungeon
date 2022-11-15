using System;
using System.Collections.Generic;
using System.Text;

namespace StarterGame
{
     public interface IWorldEvent
     {

          Room Trigger { get; }
          void Execute();
     }
     
     public interface IRoomDelegate
     {
          Room ContainingRoom { set; get; }
          Door GetExit(string exitName);
          string GetExits();
          string Description();
     }

     public interface IClosable
     {
          bool IsClosed { get; }
          bool IsOpen { get; }
          bool Open();
          bool Close();
     }

     public interface ILockable
     {
          bool IsLocked { get; }
          bool IsUnlocked { get; }
          bool Lock();
          bool Unlock();
          bool CanOpen { get; }
          bool CanClose { get; }
     }

     public interface IItem
     {
          string Name { get; }
          string LongName { get; }
          float Weight { get; }
          float Value { get; }
          string Description { get; }
          bool IsDecorator { get; set; }
          void AddDecorator(IItem decorator);
     }

}