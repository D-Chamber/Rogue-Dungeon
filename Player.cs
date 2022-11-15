using System.Collections;
using System.Collections.Generic;
using System;

namespace StarterGame
{
    public class Player
    {
        private Room _currentRoom = null;
        public Room CurrentRoom
        {
            get
            {
                return _currentRoom;
            }
            set
            {
                _currentRoom = value;
            }
        }

        public Player(Room room)
        {
            _currentRoom = room;
        }

        public void WaltTo(string direction)
        {
            Door door = this.CurrentRoom.GetExit(direction);
            if (door != null)
            {
                if (door.IsOpen)
                {
                    Room nextRoom = door.RoomOnTheOtherSideOf(CurrentRoom);
                    NotificationCenter.Instance.PostNotification(new Notification("PlayerWillEnterRoom", this));
                    this.CurrentRoom = nextRoom;
                    NotificationCenter.Instance.PostNotification(new Notification("PlayerDidEnterRoom", this));
                    this.OutputMessage("\n" + this.CurrentRoom.Description());
                }
                else
                {
                    OutputMessage("\nThe door on the " + direction + " is closed.");
                }
                
            }
            else
            {
                this.OutputMessage("\nThere is no door on " + direction);
            }
        }

        public void Say(string word)
        {
            OutputMessage("\nYou said " + word);
            Dictionary<string, Object> userInfo = new Dictionary<string, Object>();
            userInfo["word"] = word;
            Notification notification = new Notification("PlayerDidSayWord", this, userInfo);
            NotificationCenter.Instance.PostNotification(notification);
        }

        public void Inspect(string itemName)
        {
            IItem item = CurrentRoom.PickUp(itemName);
            if (item != null)
            {
                OutputMessage("\n" + item.Description);
                CurrentRoom.Drop(item);
            }
            else
            {
                OutputMessage("\nThere is no " + itemName + " in the room.");
            }
        }

        public void Open(string doorName)
        {
            Door door = CurrentRoom.GetExit(doorName);
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

        public void OutputMessage(string message)
        {
            Console.WriteLine(message);
        }
    }

}
