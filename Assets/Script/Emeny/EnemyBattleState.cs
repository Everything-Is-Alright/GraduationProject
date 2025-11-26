using UnityEngine;

public class EnemyBattleState : EntityState<Skeleton>
{
    private Transform player;
    public EnemyBattleState(Skeleton skeleton, StateMachine<Skeleton> stateMachine, string animBoolName) : base(skeleton, stateMachine, animBoolName)
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
        if (player == null && !entity.cliffCheck)
        {
            entity.SetVelocity(0, entity.rb.linearVelocity.y); 
            return; 
        }
        else if (WithinAttackRange())
        {
            stateMachine.ChangeState(entity.AttackState);
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
