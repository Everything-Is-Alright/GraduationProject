using UnityEngine;

public class SkeletonDeathState : EntityState<Skeleton>
{
    public SkeletonDeathState(Skeleton entity, StateMachine<Skeleton> stateMachine, string animBoolName) : base(entity, stateMachine, animBoolName)
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
