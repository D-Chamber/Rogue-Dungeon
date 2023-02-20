namespace RogueDungeon.Commands;

public class ShowStatCommand : Command
{
    public ShowStatCommand() : base()
    {
        this.Name = "stat";
        this.HelpText = "\nShows your stats (this command takes no arguments)." +
                        "\n\nUsage: stat\n";
    }

    public override bool Execute(Player player)
    {
        if (this.HasSecondWord())
        {
            player.OutputMessage("Show what stat?");
        }
        else
        {
            player.ShowStatus();
        }
        return false;
    }
}