namespace RogueDungeon.Commands;

public class DropCommand : Command
{
    public DropCommand() : base()
    {
        this.Name = "drop";
        this.HelpText = "\nDrops an item if it is in the inventory in the current room." +
                        "\n\nUsage: drop <item>\n";
    }

    public override bool Execute(Player player)
    {
        if (this.HasSecondWord())
        {
            player.DropItem(this.SecondWord);
        }
        else
        {
            player.OutputMessage("\nDrop what?");
        }

        return false;
    }

}