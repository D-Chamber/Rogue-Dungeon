namespace RogueDungeon.Commands;

public class AttackCommand : Command
{
    public AttackCommand() : base()
    {
        this.Name = "attack";
        this.HelpText = "\nAttack the target" +
                        "\n\nUsage: attack \n";
    }

    public override bool Execute(Player player)
    {
        player.Attack();

        return false;
    }
}