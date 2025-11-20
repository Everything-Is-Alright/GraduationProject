using UnityEngine;

public class StateMachine<T> where T : IEntity
{
    public EntityState<T> currentState {  get; private set; }

    public void Initialize(EntityState<T> startState)
    {
        currentState = startState;
        currentState.Enter();
    }

    public void ChangeState(EntityState<T> newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void UpdateActiveState()
    {
        currentState.Update();
    }
}
