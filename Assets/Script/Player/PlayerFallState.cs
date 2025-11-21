using UnityEngine;

public class PlayerFallState : PlayerAirState
{
    public PlayerFallState(Player player, StateMachine<Player> stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
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

        if(entity.groundDetected)
        {
            stateMachine.ChangeState(entity.IdleState);
        }

        if(entity.wallDetected)
        {
            stateMachine.ChangeState(entity.WallSlideState);
        }
    }
}
