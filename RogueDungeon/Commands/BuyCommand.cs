namespace RogueDungeon.Commands;

public class BuyCommand : Command
{
    public BuyCommand() : base()
    {
        this.Name = "buy";
        this.HelpText = "\nBuy an item from the shop." +
                        "\n\nUsage: buy (prompts for npc) : buy <item name> : buy <consumable> <amount>\n";
    }

    public override bool Execute(Player player)
    {
        if (this.HasSecondWord() && !this.HasThirdWord())
        {
            player.Buy(SecondWord);
        }

        else
        {
            player.Buy();
        }

        return false;
    }
}