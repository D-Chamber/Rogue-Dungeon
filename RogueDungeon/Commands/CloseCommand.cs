namespace RogueDungeon.Commands;

public class CloseCommand : Command
{
    public CloseCommand() : base()
    {
        this.Name = "close";
        this.HelpText = "\nCloses the door of a room connected to the current room." +
                        "\n\nUsage: close <direction>\n";
    }

    public override bool Execute(Player player)
    {
        if (this.HasSecondWord())
        {
            player.Close(this.SecondWord);
        }

        else
        {
            player.OutputMessage("\nOpen what?");
        }

        return false;
    }
}