using Unity.VisualScripting;
using UnityEngine;

public class SkeletonWalkState : EntityState<Skeleton>
{
    public SkeletonWalkState(Skeleton skeleton, StateMachine<Skeleton> stateMachine, string animBoolName) : base(skeleton, stateMachine, animBoolName)
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

        entity.SetVelocity(entity.moveSpeed * entity.entityFacing, entity.rb.linearVelocityY);

        if (!entity.cliffCheck)
        {
            stateMachine.ChangeState(entity.IdleState);
        }
    }
}
