using UnityEngine;

public class PlayerWallJumpState : EntityState<Player>
{
    public PlayerWallJumpState(Player player, StateMachine<Player> stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        entity.SetVelocity(-entity.playerFacing * entity.movespeed, entity.jumpspeed);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (entity.wallDetected)
        {
            stateMachine.ChangeState(entity.WallSlideState);
        }

        if (entity.groundDetected)
        {
            stateMachine.ChangeState(entity.IdleState);
        }

        if(entity.input.Player.Dash.WasPressedThisFrame())
        {
            stateMachine.ChangeState(entity.DashState);
        }
    }
}
