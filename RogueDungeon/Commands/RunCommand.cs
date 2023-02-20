namespace RogueDungeon.Commands;

public class RunCommand : Command
{
    public RunCommand() : base()
    {
        this.Name = "run";
        this.HelpText = "\nLeaves from the fight or shop." +
                        "\n\nUsage: run\n";
    }

    public override bool Execute(Player player)
    {
        player.ExitState();

        return false;
    }
}