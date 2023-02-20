using RogueDungeon.Creations;

namespace RogueDungeon.Interfaces;

public interface IWorldEvent
{
    Room Trigger { get; }
    void Execute();
}

public interface IRoomDelegate
{
    Room ContainingRoom { get; set; }
    Door GetExit(string exitName);
    string GetExits();
    string Description();
}

public interface IClosable
{
    bool IsClosed { get; }
    bool IsOpen { get; }
    bool Open();
    bool Close();
}

public interface ILockable
{
    bool IsLocked { get; }
    bool IsUnlocked { get; }
    bool Lock();
    bool Unlock();
    bool CanOpen { get; }
    bool CanClose { get; }
}

public interface IItem
{
    string Name { get; }
    string LongName { get; }
    float Weight { get; }
    float Value { get; }
    int Count { get; set; }
    string Description { get; }
    bool IsDecorator { get; }
    void AddDecorator(IItem decorator);
}

public interface IItemContainer : IItem
{
    void AddItem(IItem item);
    IItem RemoveItem(string itemName);
    // bool Interact();
}

public interface ICharacter
{
    IItemContainer _itemContainer { get; set; }
    string Name { get; set; }
    Dictionary<string, int> Stats { get; set; }
    float Gold { get; set; }
    bool IsTrader { get; }
    void Trade();
    void Buy(IItem item);
    IItem Sell(string itemName);
    void Give(IItem item);
    IItem Take(string itemName);
    void SetRoom(Room room);

    string Dialogue { get; set; }
}

public interface IConsumable : IItem
{
    int[] Modifier { get; set; }
}

public interface IWeapon : IItem
{
    int[] DamageMod { get; set; }
}

public interface IArmor : IItem
{
    string ArmorType { get; set; }
    int[] ArmorMod { get; set; }
}