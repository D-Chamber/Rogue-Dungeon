using RogueDungeon.Interfaces;

namespace RogueDungeon.Creations;

public class Door : IClosable
{
    protected Room _roomA;
    protected Room _roomB;
    private bool _closed;
    private ILockable _lock;

    public ILockable TheLock
    {
        get { return _lock; }
        set => _lock = value;
    }

    public Door(Room roomA, Room roomB)
    {
        _roomA = roomA;
        _roomB = roomB;
        _closed = false;
        TheLock = null;
    }

    public Room RoomOnTheOtherSide(Room room)
    {
        Room theOtherRoom = null;
        if (room == _roomA)
            theOtherRoom = _roomB;
        if (room == _roomB)
            theOtherRoom = _roomA;

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
        var result = false;
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
        var result = true;
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
        return result;
    }

    public void Lock()
    {
        if (TheLock == null)
            return;
        TheLock.Lock();
    }

    public void Unlock()
    {
        if (TheLock == null)
            return;
        TheLock.Lock();
    }

    // Door factory
    public static Door CreateDoor(Room roomA, Room roomB, string toRoomB, string toRoomA)
    {
        var door = new Door(roomA, roomB);
        roomA.SetExit(toRoomB, door);
        roomB.SetExit(toRoomA, door);
        
        return door;
    }
    
    public static Door CreateDoor(Door door, string toRoomB, string toRoomA)
    {
        var doors = door;
        door._roomA.SetExit(toRoomB, door);
        door._roomB.SetExit(toRoomA, door);
        
        return door;
    }
}