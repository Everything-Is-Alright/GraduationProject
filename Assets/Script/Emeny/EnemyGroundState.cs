using UnityEngine;

public class EnemyGroundState : EntityState<Skeleton>
{
    public EnemyGroundState(Skeleton skeleton, StateMachine<Skeleton> stateMachine, string animBoolName) : base(skeleton, stateMachine, animBoolName)
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
