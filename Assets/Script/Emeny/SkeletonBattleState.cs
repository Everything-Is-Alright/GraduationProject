using UnityEngine;

public class SkeletonBattleState : EntityState<Skeleton>
{
    private Transform player;
    private float lastTimeInBattleState;
    public SkeletonBattleState(Skeleton skeleton, StateMachine<Skeleton> stateMachine, string animBoolName) : base(skeleton, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if(player == null)
        {
            player = entity.PlayerDetection().transform;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(entity.entityFacing != DirectionToPlayer())
        {
            entity.Flip();
        }

        if(entity.PlayerDetection() == true)
        {
            UpdateBattleTimer();
        }

        if(BattleTimeIsOver())
        {
            entity.stateMachine.ChangeState(entity.IdleState);
        }

        if(CouldRetreat())
        {
            entity.rb.linearVelocity = new Vector2(entity.retreatVelocity.x * DirectionToPlayer(), entity.retreatVelocity.y);
            entity.HandleFlip(DirectionToPlayer());
        }

        if (player == null || !entity.cliffCheck)
        {
            entity.SetVelocity(0, entity.rb.linearVelocity.y); 
            return; 
        }
        else if (WithinAttackRange())
        {
            stateMachine.ChangeState(entity.AttackState);
            return;
        }
        else
        {
            entity.SetVelocity(entity.battleMoveSpeed * DirectionToPlayer(), entity.rb.linearVelocity.y);
        }
    }

    private float DistanceToPlayer()
    {
        if(player == null)
        {
            return float.MaxValue;
        }

        return Mathf.Abs(player.position.x - entity.transform.position.x);
    }

    private void UpdateBattleTimer() => lastTimeInBattleState = Time.time;
    private bool BattleTimeIsOver() => Time.time > lastTimeInBattleState + entity.battleTimerDuration;
    private bool CouldRetreat() => DistanceToPlayer() < entity.retreatDistance;
    private bool WithinAttackRange() => DistanceToPlayer() < entity.attackDistance;

    private int DirectionToPlayer()
    {
        if(player == null)
        {
            return 0;
        }

        return player.position.x > entity.transform.position.x ? 1 : -1;
    }

}
