using RogueDungeon.Interfaces;

namespace RogueDungeon.Logic;

public class PlayerState
{
    private State _state = null;

    public PlayerState(State state)
    {
        this.TransitionTo(state);
    }

    public void TransitionTo(State state)
    {
        this._state = state;
        this._state.SetContext(this);
    }

    public void Update(Player player)
    {
        this._state.Update(player);
    }
}