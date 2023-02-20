namespace RogueDungeon.Commands;

public class PickupCommand : Command
{
    public PickupCommand() : base()
    {
        this.Name = "pickup";
        this.HelpText = "\nPicks up an item available in the room." +
                        "\n\nUsage: pickup <item>\n";
    }

    public override bool Execute(Player player)
    {
        if (this.HasSecondWord() && !HasThirdWord())
        {
            player.Pickup(this.SecondWord);
        }
        else if (HasSecondWord() && HasThirdWord())
        {
            player.Pickup(SecondWord + " " + ThirdWord);
        }
        else
        {
            player.OutputMessage("\nPickup what?");
        }

        return false;
    }
}