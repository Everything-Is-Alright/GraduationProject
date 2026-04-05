using UnityEngine;

public class EyesIdleState : EntityState<Eyes>
{
    private float IdleTimer = 3f;

    public EyesIdleState(Eyes eyes, StateMachine<Eyes> stateMachine, string animBoolName) : base(eyes, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        entity.stateTimer = IdleTimer;
        entity.SetVelocity(0, entity.rb.linearVelocityY);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        entity.stateTimer -= Time.deltaTime;

        if (entity.stateTimer < 0)
        {
            
            // 切换到FlyState
            stateMachine.ChangeState(entity.FlyState);
        }
    }
}
