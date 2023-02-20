namespace RogueDungeon.Commands;

public class TradeCommand : Command
{
    public TradeCommand() : base()
    {
        this.Name = "trade";
        this.HelpText = "\nTrade with another character (or NPC). " +
                        "Using one parameter calls shows what the character has to offer in their inventory.\n" +
                        "The alternative call after you have seen whats in their inventory is to pass two arguments." +
                        "\n\nUsage: trade <character name> || trade <playerItem> <npcItem> \n";
    }

    public override bool Execute(Player player)
    {
        if (this.HasSecondWord() && !this.HasThirdWord())
        {
            player.Trade(SecondWord);
        }
        
        else if (this.HasSecondWord() && this.HasThirdWord())
        {
            player.Trade(SecondWord, ThirdWord);
        }
        
        else
        {
            player.OutputMessage("\nTrade with whom?");
        }

        return false;
    }
}