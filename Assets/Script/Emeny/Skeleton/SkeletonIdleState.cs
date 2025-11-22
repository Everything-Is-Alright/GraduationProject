using UnityEngine;

public class SkeletonIdleState : EntityState<Skeleton>
{
    private float IdleTimer = 3f;

    public SkeletonIdleState(Skeleton skeleton, StateMachine<Skeleton> stateMachine, string animBoolName) : base(skeleton, stateMachine, animBoolName)
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

        Debug.Log(entity.groundDetected);

        if (entity.stateTimer < 0)
        {
            if(!entity.cliffCheck)
            {
                entity.Flip();
            }
            stateMachine.ChangeState(entity.WalkState);
        }
    }
}
