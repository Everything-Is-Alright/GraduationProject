using UnityEngine;

public class SkeletonWalkState : EntityState<Skeleton>
{
    public SkeletonWalkState(Skeleton entity, StateMachine<Skeleton> stateMachine, string animBoolName) : base(entity, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        entity.SetVelocity(entity.moveSpeed, entity.rb.linearVelocityY);
    }
}
