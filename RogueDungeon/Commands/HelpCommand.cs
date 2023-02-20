using Pastel;
using RogueDungeon.Logic;

namespace RogueDungeon.Commands;

public class HelpCommand : Command
{
    private CommandWords _words;

    public HelpCommand() : this(new CommandWords()){}

    // Designated Constructor
    public HelpCommand(CommandWords commands) : base()
    {
        _words = commands;
        this.Name = "help";
    }

    override
        public bool Execute(Player player)
    {
        if (this.HasSecondWord())
        {
            // print the help for the command if it is available
            if (_words.IsCommand(this.SecondWord))
            {
                player.OutputMessage(_words.GetCommand(this.SecondWord).HelpText);
            }
            else
            {
                player.OutputMessage("\nI cannot help you with " + this.SecondWord);            
            }
        }
        else
        {
            player.OutputMessage(("\nYou are in a dungeon type structure lost and alone. If you can find your goal and survive\n" +
                                 "you may be able to make it back alive. ** Possibly with some riches **" +
                                 "\n\nYour available commands are:\n" +
                                 _words.Description() + "\n\nType 'help <command>' for more information.").Pastel("#ff991e"));
        }
        return false;
    }
}