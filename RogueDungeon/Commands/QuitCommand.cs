namespace RogueDungeon.Commands;

public class QuitCommand : Command
{
    public QuitCommand() : base()
    {
        this.Name = "quit";
    }

    public override bool Execute(Player player)
    {
        if (this.HasSecondWord())
        {
            player.OutputMessage("\nI cannot quit " + this.SecondWord);
            return false;
        }

        return true;
    }
    
}