using RogueDungeon.Commands;
using RogueDungeon.Logic;
using RogueDungeon.Creations;

namespace RogueDungeon.Interfaces;

public abstract class State
{
    protected PlayerState _context;
    
    public void SetContext(PlayerState context)
    {
        _context = context;
    }
    
    public abstract void ChangeState(State state);
    
    public abstract void Update(Player player);
}

public class ShoppingState : State
{
    public override void ChangeState(State state)
    {
        _context.TransitionTo(state);
    }

    public override void Update(Player player)
    {
        Command[] commands = { new TradeCommand(), new BuyCommand(), new SellCommand(),  new RunCommand(), new ShowInventoryCommand() };
        var tradeCommands = new CommandWords(commands);

        var userInfo = new Dictionary<string, object>();
        userInfo["commands"] = tradeCommands;
        var notification = new Notification("ChangeState", player, userInfo);
        NotificationCenter.Instance.PostNotification(notification);
    }
}

public class RunningState : State
{
    public override void ChangeState(State state)
    {
        _context.TransitionTo(state);
    }

    public override void Update(Player player)
    {
        Command[] commands =
        {
            new BackCommand(),
            new CloseCommand(),
            new DropCommand(),
            new EquipCommand(),
            new GoCommand(),
            new QuitCommand(),
            new InspectCommand(),
            new OpenCommand(),
            new PickupCommand(),
            new RunCommand(),
            new SayCommand(),
            new ShowInventoryCommand(),
            new ShowStatCommand(),
            new TalkCommand(),
            new UseCommand()
        };
        var runningCommands = new CommandWords(commands);

        var userInfo = new Dictionary<string, object>();
        userInfo["commands"] = runningCommands;
        var notification = new Notification("ChangeState", player, userInfo);
        NotificationCenter.Instance.PostNotification(notification);
    }
}

public class FightingState : State
{
    public override void ChangeState(State state)
    {
        _context.TransitionTo(state);
    }

    public override void Update(Player player)
    {
        Command[] commands = { 
            new AttackCommand(),
            new DefendCommand(),
            new UseCommand(),
            new RunCommand(),
            new ShowStatCommand(),
            new ShowInventoryCommand()
        };
        var fightCommand = new CommandWords(commands);

        var userInfo = new Dictionary<string, object>();
        userInfo["commands"] = fightCommand;
        var notification = new Notification("ChangeState", player, userInfo);
        NotificationCenter.Instance.PostNotification(notification);
    }
}

public class DeadState : State
{
    public override void ChangeState(State state)
    {
        _context.TransitionTo(state);
    }

    public override void Update(Player player)
    {
        Command[] commands = { new QuitCommand() };
        var deadCommands = new CommandWords(commands);

        var userInfo = new Dictionary<string, object>();
        userInfo["commands"] = deadCommands;
        var notification = new Notification("ChangeState", player, userInfo);
        NotificationCenter.Instance.PostNotification(notification);
    }
}

public class WinState : State
{
    public override void ChangeState(State state)
    {
        _context.TransitionTo(state);
    }

    public override void Update(Player player)
    {
        Command[] commands = { new QuitCommand() };
        var winCommands = new CommandWords(commands);

        var userInfo = new Dictionary<string, object>();
        userInfo["commands"] = winCommands;
        var notification = new Notification("ChangeState", player, userInfo);
        NotificationCenter.Instance.PostNotification(notification);
    }
}