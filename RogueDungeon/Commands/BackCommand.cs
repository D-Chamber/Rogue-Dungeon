namespace RogueDungeon.Commands;

public class BackCommand : Command
{
    public BackCommand() : base()
    {
        this.Name = "back";
        this.HelpText = "Go back to the previous room.\n" +
                        "Usage: back";
    }

    public override bool Execute(Player player)
    {
        if (this.HasSecondWord())
        {
            player.OutputMessage("Back where? (Invalid Input)");
        }
        else
        {
            player.GoBack();
        }
        
        return false;
    }
}