namespace RogueDungeon.Commands;

public class TalkCommand : Command
{
    public TalkCommand() : base()
    {
        this.Name = "talk";
        this.HelpText = "Talk to someone in the room." +
                        "\n\nUsage: talk <character name>";
    }

    public override bool Execute(Player player)
    {
        if (HasSecondWord())
        {
            player.Talk(SecondWord);
        }
        
        else 
        {
            player.Talk();
        }

        return false;
    }
}