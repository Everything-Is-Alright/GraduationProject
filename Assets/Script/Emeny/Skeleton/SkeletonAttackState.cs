using UnityEngine;

public class SkeletonAttackState : EntityState<Skeleton>
{

    public float attackCooldown = 0.8f;
    public float attackCooldownTimer;
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
        entity.SetVelocity(0, 0);
        if (triggerCalled)
        {
            stateMachine.ChangeState(entity.BattleState);
        }
    }
}
