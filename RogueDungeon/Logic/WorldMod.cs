using RogueDungeon.Interfaces;
using RogueDungeon.Creations;

namespace RogueDungeon;

public class WorldMod : IWorldEvent
{
    private Room _trigger;
    private Room _sideA;
    private Room _sideB;
    private string _toSideA;
    private string _toSideB;
    
    public Room Trigger
    {
        get { return _trigger; }
    }
    
    public WorldMod(Room trigger, Room sideA, Room sideB, string toSideB, string toSideA)
    {
        _trigger = trigger;
        _sideA = sideA;
        _sideB = sideB;
        _toSideA = toSideA;
        _toSideB = toSideB;
    }

    public void Execute()
    {
        var door = Door.CreateDoor(_sideA, _sideB, _toSideB, _toSideA);
        door.Close();
    }
}