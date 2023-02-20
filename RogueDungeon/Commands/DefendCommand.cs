namespace RogueDungeon.Commands;

public class DefendCommand : Command
{
    public DefendCommand() : base()
    {
        this.Name = "defend";
        this.HelpText = "\nDefend yourself from an attack" +
                        "\n\nUsage: defend\n";
    }

    public override bool Execute(Player player)
    {
        player.Defend();

        return false;
    }
}