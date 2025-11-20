using UnityEngine;

public class PlayerWallJumpState : EntityState<Player>
{
    public PlayerWallJumpState(Player player, StateMachine<Player> stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetVelocity(-player.playerFacing * player.movespeed, player.jumpspeed);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (player.wallDetected)
        {
            stateMachine.ChangeState(player.WallSlideState);
        }

        if (player.groundDetected)
        {
            stateMachine.ChangeState(player.IdleState);
        }

        if(player.input.Player.Dash.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.DashState);
        }
    }
}
