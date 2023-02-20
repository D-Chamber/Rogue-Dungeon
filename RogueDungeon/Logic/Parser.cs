using RogueDungeon.Commands;

namespace RogueDungeon.Logic
{
    public class Parser
    {
        private Stack<CommandWords> _commands;

        public Parser() : this(new CommandWords()){}

        // Designated Constructor
        public Parser(CommandWords newCommands)
        {
            _commands = new Stack<CommandWords>(); 
            _commands.Push(newCommands);
            NotificationCenter.Instance.AddObserver("ChangeState", ChangeCommandWords);
            NotificationCenter.Instance.AddObserver("LeavingState", PopCommandWords);
        }

        public Command ParseCommand(string commandString)
        {
            Command command = null;
            string[] words = commandString.Split(' ');
            if (words.Length > 0)
            {
                command = _commands.Peek().Get(words[0]);
                if (command != null)
                {
                    if (words.Length > 1)
                    {
                        command.SecondWord = words[1];
                        if (words.Length > 2)
                        {
                            command.ThirdWord = words[2];
                        }
                        else
                        {
                            command.ThirdWord = null;
                        }
                    }
                    else
                    {
                        command.SecondWord = null;
                    }
                }
                else
                {
                    // This is debug line of code, should remove for regular execution
                    Console.WriteLine(">>>Did not find the command " + words[0]);
                }
            }
            else
            {
                // This is a debug line of code
                Console.WriteLine("No words parsed!");
            }
            return command;
        }

        public string Description()
        {
            return _commands.Peek().Description();
        }
        
        private void ChangeCommandWords(Notification notification)
        {
            if (notification.UserInfo["commands"] is CommandWords commands)
            {
                _commands.Push(commands);
            }
        }
        
        private void PopCommandWords(Notification notification)
        {
            _commands.Pop();
        }
    }
}