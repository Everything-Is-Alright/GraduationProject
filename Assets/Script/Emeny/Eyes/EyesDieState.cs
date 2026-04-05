using UnityEngine;

public class EyesDieState : EntityState<Eyes>
{
    public EyesDieState(Eyes eyes, StateMachine<Eyes> stateMachine, string animBoolName) : base(eyes, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        entity.SetVelocity(0, 0);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}
