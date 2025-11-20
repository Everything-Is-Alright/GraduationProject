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

        if(player.groundDetected)
        {
            stateMachine.ChangeState(player.IdleState);
        }

        if(player.wallDetected)
        {
            stateMachine.ChangeState(player.WallSlideState);
        }
    }
}
