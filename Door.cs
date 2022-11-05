namespace StarterGame;

public class Door
{
    private Room _roomA;
    private Room _roomB;

    public Door(Room roomA, Room roomB)
    {
        _roomA = roomA;
        _roomB = roomB;
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

    public static Door CreateDoor(Room roomA, Room roomB, string toRoomA, string toRoomB)
    {
        Door door = new Door(roomA, roomB);
        roomA.SetExit(toRoomA, door);
        roomB.SetExit(toRoomB, door);

        return door;
    }
}