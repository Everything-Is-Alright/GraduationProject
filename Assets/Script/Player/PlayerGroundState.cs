using UnityEngine;

public class PlayerGroundState : EntityState
{
    public PlayerGroundState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
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

        if(player.rb.linearVelocityY < 0)
        {
            stateMachine.ChangeState(player.FallState);
        }

        if(player.stateTimer <= 0 && player.input.Player.Jump.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.JumpState);
        }

        if((player.stateTimer <= 0 && player.input.Player.Roll.WasPressedThisFrame()) && player.groundDetected)
        {
            stateMachine.ChangeState(player.RollState);
        }

        if((player.stateTimer <= 0 && player.input.Player.Dash.WasPressedThisFrame()))
        {
            stateMachine.ChangeState(player.DashState);
        }
    }
}
