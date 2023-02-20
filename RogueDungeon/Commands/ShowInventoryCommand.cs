namespace RogueDungeon.Commands;

public class ShowInventoryCommand : Command
{
    public ShowInventoryCommand() : base()
    {
        this.Name = "inventory";
        this.HelpText = "\nShows you items currently in your inventory (this command takes no arguments)." +
                        "\n\nUsage: inventory\n";
    }

    public override bool Execute(Player player)
    {
        if (!this.HasSecondWord())
        {
            player.DisplayInventory();
        }
        else
        {
            player.OutputMessage("\nIncorrect argument for this command.");
        }

        return false;
    }
}