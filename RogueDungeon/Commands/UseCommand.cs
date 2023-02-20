namespace RogueDungeon.Commands;

public class UseCommand : Command
{
    public UseCommand() : base()
    {
        this.Name = "use";
        this.HelpText = "\nUse an item." +
                        "\n\nUsage: use <consumable>";
    }

    public override bool Execute(Player player)
    {
        if (HasSecondWord() && !HasThirdWord())
        {
            player.Use(SecondWord);
        }
        
        else if (HasSecondWord() && HasThirdWord())
        {
            player.Use(SecondWord, ThirdWord);
        }

        else
        {
            player.OutputMessage("\nUse what?");
        }

        return false;
    }
}