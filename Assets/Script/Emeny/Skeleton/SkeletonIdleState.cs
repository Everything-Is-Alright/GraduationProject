using UnityEngine;

public class SkeletonIdleState : SkeletonGroundState
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
        Debug.Log("Ω¯»Î’æ¡¢");

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
            if(!entity.cliffCheck)
            {
                entity.Flip();
            }
            stateMachine.ChangeState(entity.WalkState);
        }
    }
}
