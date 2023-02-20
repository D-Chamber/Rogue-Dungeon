namespace RogueDungeon.Commands;

public class EquipCommand : Command
{
    public EquipCommand() : base()
    {
        this.Name = "equip";
        this.HelpText = "\nEquip a piece of gear from your inventory. " +
                        "If the gear is already equipped the command will also un-equip the item for you." +
                        "\n\nUsage: equip <gear name>\n";
    }

    public override bool Execute(Player player)
    {
        if (this.HasSecondWord() && !HasThirdWord())
        {
            player.Equip(this.SecondWord);
        }
        else if (HasSecondWord() && HasThirdWord())
        {
            player.Equip(SecondWord + " " + ThirdWord);
        }
        else
        {
            player.OutputMessage("\nEquip what?");
        }

        return false;
    }
}