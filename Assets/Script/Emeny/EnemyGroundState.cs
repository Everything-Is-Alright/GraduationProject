using UnityEngine;

public class EnemyGroundState : EntityState<Skeleton>
{
    public EnemyGroundState(Skeleton entity, StateMachine<Skeleton> stateMachine, string animBoolName) : base(entity, stateMachine, animBoolName)
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

        if(entity.PlayerDetection() == true)
        {
            stateMachine.ChangeState(entity.BattleState);
        }
    }
}
