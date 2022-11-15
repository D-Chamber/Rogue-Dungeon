using System.ComponentModel.Design.Serialization;

namespace StarterGame;

public class Door : IClosable
{
    private Room _roomA;
    private Room _roomB;
    private bool _closed;
    private ILockable _lock;
    
    public ILockable TheLock
    {
        get { return _lock; }
        set => _lock = value;
    }

    private Door(Room roomA, Room roomB)
    {
        _roomA = roomA;
        _roomB = roomB;
        _closed = false;
        TheLock = null;
    }

    public Room RoomOnTheOtherSideOf (Room room)
    {
        Room theOtherRoom = null;
        if (room == _roomA)
        {
            theOtherRoom = _roomB;
        }

        if (room == _roomB)
        {
            theOtherRoom = _roomA;
        }

        return theOtherRoom;
    }
    
    public bool IsOpen
    {
        get { return !_closed; }
    }
    public bool IsClosed
    {
        get { return _closed; }
    }

    public bool Open()
    {
        bool result = false;
        if (TheLock != null)
        {
            if (TheLock.CanOpen)
            {
                _closed = false;
                result = true;
            }
        }
        else
        {
            _closed = false;
            result = true;
        }
        return result;
    }

    public bool Close()
    {
        bool result = false;
        if (TheLock != null)
        {
            if (TheLock.CanClose)
            {
                _closed = true;
                result = true;
            }
        }
        else
        {
            _closed = true;
            result = true;
        }
        _closed = true;
        return result;
    }

    public static Door CreateDoor(Room roomA, Room roomB, string toRoomB, string toRoomA)
    {
        var door = new Door(roomA, roomB);
        roomA.SetExit(toRoomB, door);
        roomB.SetExit(toRoomA, door);

        return door;
    }
}