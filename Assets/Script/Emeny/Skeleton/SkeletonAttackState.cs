using UnityEngine;

public class SkeletonAttackState : EntityState<Skeleton>
{
    public SkeletonAttackState(Skeleton skeleton, StateMachine<Skeleton> stateMachine, string animBoolName) : base(skeleton, stateMachine, animBoolName)
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
    }
}
