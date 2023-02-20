namespace RogueDungeon.Commands;

public abstract class Command
{
    public string Name { get; set; }
    public string SecondWord { get; set; }
    public string HelpText { get; set; }
    public string ThirdWord { get; set; }

    protected Command()
    {
        this.Name = "";
        this.SecondWord = null;
    }

    protected bool HasSecondWord()
    {
        return this.SecondWord != null;
    }

    protected bool HasThirdWord()
    {
        return this.ThirdWord != null;
    }

    public abstract bool Execute(Player player);
}