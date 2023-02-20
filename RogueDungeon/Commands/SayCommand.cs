namespace RogueDungeon.Commands;

public class SayCommand : Command
{
    public SayCommand() : base()
    {
        this.Name = "say";
        this.HelpText = "Say's a word out loud. (Used for verbal passwords or just plain interaction)" +
                        "\nUsage: say <word>";
    }

    public override bool Execute(Player player)
    {
        if (this.HasSecondWord())
        {
            player.Say(this.SecondWord);
        }
        else
        {
            player.Say("What do you want me to say?");
        }
        
        return false;
    }
}