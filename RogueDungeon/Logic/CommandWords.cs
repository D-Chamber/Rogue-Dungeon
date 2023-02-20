using RogueDungeon.Commands;

namespace RogueDungeon.Logic
{
    public class CommandWords
    {
        private Dictionary<string, Command> commands;
        private static Command[] commandArray =
        {
            new GoCommand(),
            new QuitCommand(),
            new InspectCommand(),
            new OpenCommand(),
            new PickupCommand(),
            new ShowInventoryCommand(),
            new CloseCommand(),
            new DropCommand(),
            new ShowStatCommand(),
            new BackCommand(),
            new SayCommand(),
            new TradeCommand(),
            new EquipCommand(),
            new SellCommand(),
            new BuyCommand(),
            new UseCommand(),
            new TalkCommand(),
            new RunCommand()
        };

        public CommandWords() : this(commandArray) {}

        // Designated Constructor
        public CommandWords(Command[] commandList)
        {
            commands = new Dictionary<string, Command>();
            foreach (Command command in commandList)
            {
                commands[command.Name] = command;
            }
            var help = new HelpCommand(this);
            commands[help.Name] = help;
        }

        public Command Get(string word)
        {
            Command command = null;
            commands.TryGetValue(word, out command);
            return command;
        }

        public string Description()
        {
            string commandNames = "";
            Dictionary<string, Command>.KeyCollection keys = commands.Keys;
            foreach (string commandName in keys)
            {
                commandNames += " " + commandName;
            }
            return commandNames;
        }

        public Command? GetCommand(string secondWord)
        {
            commands.TryGetValue(secondWord, out Command command);
            return command;
        }

        public bool IsCommand(string secondWord)
        {
            return commands.ContainsKey(secondWord);
        }
    }
}