using System.Collections;
using System.Collections.Generic;
using System;

namespace StarterGame
{
    public interface IRoomDelegate
    {
        Room ContainingRoom { set; get; }
        Room GetExit(string exitName);
        string GetExits();
        string Description();
    }

    public class TrapRoom : IRoomDelegate
    {
        public string UnlockWord { get; set; }
        
        public Room ContainingRoom { set; get; }

        public TrapRoom() : this("password")
        {
            
        }
        
        // desginated constructor
        public TrapRoom(string unlockWord)
        {
            UnlockWord = unlockWord;
            NotificationCenter.Instance.AddObserver("PlayerDidSayWord", PlayerDidSayWord);
        }
        public Room GetExit(string exitName)
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

        public void PlayerDidSayWord(Notification notification)
        {
            Player player = (Player)notification.Object;
            if(player != null)
            {
                if (player.CurrentRoom.Delegate == this)
                { 
                    Dictionary<string, Object> userInfo = notification.UserInfo;
                    string word = (string)userInfo["word"];
                    player.OutputMessage("You said the right word"); 
                    player.CurrentRoom.Delegate = null;
                    NotificationCenter.Instance.RemoveObserver("PlayerDidSayWord", PlayerDidSayWord);
                    player.OutputMessage("\n" + player.CurrentRoom.Description());
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
        
        public Room GetExit(string exitName)
        {
            ContainingRoom.Delegate = null;
            Room exit = ContainingRoom.GetExit(exitName);
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
        
        public void PlayerDidSayWord(Notification notification)
        {
            Player player = (Player)notification.Object;
            Dictionary<string, Object> userInfo = notification.UserInfo;
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
        private Dictionary<string, Room> _exits;
        private string _tag;
        public string Tag
        {
            get
            {
                return _tag;
            }
            set
            {
                _tag = value;
            }
        }

        private IRoomDelegate _roomDelegate;
        public IRoomDelegate Delegate 
        { 
            get { return _roomDelegate; } 
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
            _exits = new Dictionary<string, Room>();
            this.Tag = tag;
        }

        public void SetExit(string exitName, Room room)
        {
            _exits[exitName] = room;
        }

        public Room GetExit(string exitName)
        {
            Room room = null;
            if(_roomDelegate != null)
            {
                room = _roomDelegate.GetExit(exitName);
            }
            else
            {
                _exits.TryGetValue(exitName, out room);
            }
            return room;
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
                Dictionary<string, Room>.KeyCollection keys = _exits.Keys;
                foreach (string exitName in keys)
                {
                    exitNames += " " + exitName;
                }
            }

            return exitNames;
        }

        public string Description()
        {
            return _roomDelegate != null ? _roomDelegate.Description() : "You are " + this.Tag + ".\n *** " + this.GetExits();
        }
    }
}
