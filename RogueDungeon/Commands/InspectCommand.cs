namespace RogueDungeon.Commands;

public class InspectCommand : Command
{
    public InspectCommand() : base()
    {
        this.Name = "inspect";
        this.HelpText = "\nInspect an item if it is in the inventory or in the current room." +
                        "\n\nUsage: inspect <item>\n";
    }

    public override bool Execute(Player player)
    {
        if (this.HasSecondWord())
        {
            player.Inspect(SecondWord);
        }
        else
        {
            player.OutputMessage("Inspect what?");
        }

        return false;
    }
}