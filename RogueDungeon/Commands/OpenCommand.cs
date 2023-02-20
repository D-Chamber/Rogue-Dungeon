namespace RogueDungeon.Commands;

public class OpenCommand : Command
{
    public OpenCommand() : base()
    {
        this.Name = "open";
        this.HelpText = "\nOpens the door of a room connected to the current room." +
                        "\n\nUsage: open <direction>\n";
    }

    public override bool Execute(Player player)
    {
        if (this.HasSecondWord())
        {
            player.Open(this.SecondWord);
        }

        else
        {
            player.OutputMessage("\nOpen what?");
        }

        return false;
    }
}