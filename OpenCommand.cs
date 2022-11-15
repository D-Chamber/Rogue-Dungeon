using System.Buffers.Text;

namespace StarterGame;

public class OpenCommand : Command
{
    public OpenCommand() : base()
    {
        this.Name = "open";
    }

    override
        public bool Execute(Player player)
    {
        if (this.HasSecondWord())
        {
            player.Open(this.SecondWord);
        }
        
        else
        {
            player.OutputMessage("\nOpen what?");
        }
        return false;
    }
}