using RogueDungeon.Interfaces;

namespace RogueDungeon.Logic;

public class RegularLock : ILockable
{
    private bool _isLocked;

    public RegularLock()
    {
        _isLocked = false;
    }
    
    public bool IsLocked => _isLocked;

    public bool IsUnlocked => !_isLocked;

    public bool CanOpen => _isLocked ? false : true;

    public bool CanClose => true;

    public bool Lock()
    {
        bool result = true;
        _isLocked = true;
        return result;
    }

    public bool Unlock()
    {
        bool result = true;
        _isLocked = false;
        return result;
    }
}

// makes a lock that can only be unlocked when the player says the password
public class PasswordLock : ILockable
{
    private bool _isLocked;

    public string Password
    {
        get;
        set;
    }
    public bool IsLocked => _isLocked;
    public bool IsUnlocked => !_isLocked;
    public bool Lock()
    {
        var result = true;
        _isLocked = true;
        return result;
    }

    public bool Unlock()
    {
        var result = true;
        _isLocked = false;
        return result;
    }

    public bool CanOpen => _isLocked ? false : true;

    public bool CanClose => true;

    public PasswordLock(string password = "password")
    {
        _isLocked = true;
        Password = password;
        NotificationCenter.Instance.AddObserver("PlayerSaidWord", PlayerSaid);
    }
    
    private void PlayerSaid(Notification notification)
    {
        var player = (Player)notification.Object;
        var Locked = player.CurrentRoom.LockCheck(this);

        if (player != null && Locked)
        {
            // makes sure word is said by player only in the room containing the locked door.
            var userInfo = notification.UserInfo;
            var word = userInfo["word"] as string;
            if (word == Password)
            {
                player.OutputMessage("You said the password correctly!");
                Unlock();
            }
            else
            {
                player.OutputMessage("That's not the correct password");
            }
        }
    }
}