namespace RogueDungeon.Commands;

public class GoCommand : Command
{
    public GoCommand() : base()
    {
        this.Name = "go";
        this.HelpText = "\nTakes you to a new location connected to the current room." +
                        "\n\nUsage: go <direction>\n";
    }

    public override bool Execute(Player player)
    {
        if (this.HasSecondWord())
        {
            player.WalkTo(this.SecondWord);
        }
        else
        {
            player.OutputMessage("\nGo where?");
        }

        return false;
    }
}