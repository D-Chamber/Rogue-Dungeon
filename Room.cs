using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

namespace StarterGame
{
    public class TrapRoom : IRoomDelegate
    {
        public string UnlockWord { get; set; }
        
        public Room ContainingRoom { set; get; }

        public TrapRoom() : this("password")
        {
            
        }
        
        // designated constructor
        public TrapRoom(string unlockWord)
        {
            UnlockWord = unlockWord;
            NotificationCenter.Instance.AddObserver("PlayerDidSayWord", PlayerDidSayWord);
        }
        public Door GetExit(string exitName)
        {
            return null;
        }
        public string GetExits() 
        {
            return "There are no exits here.";
        }
        public string Description()
        {
            return "You are in a trap room. Hah, hah.";
        }

        private void PlayerDidSayWord(Notification notification)
        {
            Player player = (Player)notification.Object;
            if(player != null)
            {
                if (player.CurrentRoom.Delegate == this)
                { 
                    Dictionary<string, object> userInfo = notification.UserInfo;
                    var word = (string)userInfo["word"];
                    if (word == UnlockWord)
                    {
                        player.OutputMessage("You said the right word"); 
                        player.CurrentRoom.Delegate = null;
                        NotificationCenter.Instance.RemoveObserver("PlayerDidSayWord", PlayerDidSayWord);
                        player.OutputMessage("\n" + player.CurrentRoom.Description());
                    }
                }
            }
        }
    }

    public class EchoRoom : IRoomDelegate
    {
        public Room ContainingRoom { get; set; }
        public EchoRoom()
        {
            NotificationCenter.Instance.AddObserver("PlayerDidSayWord", PlayerDidSayWord);
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
            string exits = "";
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
            string description = "You are in an echo room.\n";
            ContainingRoom.Delegate = null;
            description += ContainingRoom.Description();
            ContainingRoom.Delegate = this;

            return description;
        }

        private void PlayerDidSayWord(Notification notification)
        {
            var player = (Player)notification.Object;
            var userInfo = notification.UserInfo;
            string word = (string)userInfo["word"];
            if(player != null)
            {
                if (player.CurrentRoom.Delegate == this)
                {
                    if (word != null)
                    {
                        player.OutputMessage($"{word}...{word}...{word}...");
                    }
                }
            }
        }
    }

    public class Room
    {
        private Dictionary<string, Door> _exits;
        private string _tag;
        private IItem _item;
        public string Tag
        {
            get => _tag;
            set => _tag = value;
        }

        private IRoomDelegate _roomDelegate;
        public IRoomDelegate Delegate 
        { 
            get => _roomDelegate;
            set 
            {
                _roomDelegate = value;
                if (_roomDelegate != null)
                {
                    _roomDelegate.ContainingRoom = this;
                }
            } 
        }

        public Room() : this("No Tag"){}

        // Designated Constructor
        public Room(string tag)
        {
            Delegate = null;
            _exits = new Dictionary<string, Door>();
            this.Tag = tag;
            _item = null;
        }

        public void SetExit(string exitName, Door door)
        {
            _exits[exitName] = door;
        }

        public Door GetExit(string exitName)
        {
            Door door = null;
            if(_roomDelegate != null)
            {
                door = _roomDelegate.GetExit(exitName);
            }
            else
            {
                _exits.TryGetValue(exitName, out door);
            }
            return door;
        }

        public string GetExits()
        {
            string exitNames = "Exits: ";
            if(_roomDelegate != null)
            {
                exitNames += _roomDelegate.GetExits();
            }
            else
            {
                var keys = _exits.Keys;
                exitNames = keys.Aggregate(exitNames, (current, exitName) => current + (" " + exitName));
            }

            return exitNames;
        }

        public void Drop(IItem item)
        {
            _item = item;
        }

        public IItem PickUp(string itemName)
        {
            IItem itemToReturn = null;
            if (_item != null)
            {
                if (_item.Name.Equals(itemName))
                {
                    IItem tempItem = _item;
                    _item = null;
                    itemToReturn = tempItem;
                }
            }

            return itemToReturn;
        }

        public string Description()
        {
            return _roomDelegate != null ? _roomDelegate.Description() : "You are " + this.Tag + ".\n *** " + this.GetExits() 
                                                                         + "\n >>> Item: " + (_item != null ? _item.Name : "<.>");
        }
    }
}
