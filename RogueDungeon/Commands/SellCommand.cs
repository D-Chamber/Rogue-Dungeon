namespace RogueDungeon.Commands;

public class SellCommand : Command
{
    public SellCommand() : base()
    {
        this.Name = "sell";
        this.HelpText = "\nSell an item from your inventory." +
                        "\n\nUsage: sell <item name>\n";
    }

    public override bool Execute(Player player)
    {
        if (HasSecondWord())
        {
            player.Sell(SecondWord);
        }
        else
        {
            player.OutputMessage("\nSell what?\n");
        }

        return false;
    }
}