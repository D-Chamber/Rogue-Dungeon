using Pastel;
using RogueDungeon.Logic;
using RogueDungeon.Commands;

namespace RogueDungeon;

public class Game
{
    private readonly Player _player;
    private readonly Parser _parser;
    private bool _playing;

    public Game()
    {
        _playing = false;
        _parser = new Parser(new CommandWords());
        _player = new Player(GameWorld.Instance.Entrance);
    }

    /**
     *  Main play routine.  Loops until end of play.
     */
    public void Play()
    {
        NotificationCenter.Instance.AddObserver("Finished", EndGame);
        var finished = false;
        while (!finished && _playing != false)
        {
            Console.Write("\n>");
            var command = _parser.ParseCommand(Console.ReadLine());
            if (command == null)
            {
                Console.WriteLine("I don't understand...");
            }
            else
            {
                finished = command.Execute(_player);
            }
        }
    }

    public void Start()
    {
        _playing = true;
        _player.OutputMessage(Welcome());
    }

    public void End()
    {
        _playing = false;
        _player.OutputMessage(Goodbye());
    }
    
    private string Welcome()
    {
        return ("Welcome to Rogue Dungeon!\n\n" +
               "You woke up as if you were just in a nightmare. In front of you is a large door" +
               " that leads into a large Cave Entrance." +
               "\nThe door seems as if it is calling you to enter it. Whether you can make it back home or not\n" +
               "is up to you." +
               "\n\nType 'help' if you need help." +
               "\n\n" + _player.CurrentRoom.Description()).Pastel("#F7F6CF");
    }
    
    private string Goodbye()
    {
        return ("\nThanks for playing Rogue Dungeon!\n" +
               "See you next time!").Pastel("#F7F6CF");
    }
    
    private void EndGame(Notification notification)
    {
        _player.OutputMessage(("\nYou have beat the game time to head out and enjoy the rest of your life." +
                               "\nTake a breath and relax, whenever you are ready type 'quit' and the game will be over.").Pastel("#ff991e") + 
                               "\n\n--------------------THIS GAME WAS A WORK IN PROGRESS--------------------".Pastel("#f05457"));
        _player.EnterWinningState();
        _playing = false;
    }
}