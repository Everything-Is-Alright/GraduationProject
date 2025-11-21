using UnityEngine;

public class PlayerGroundState : EntityState<Player>
{
    public PlayerGroundState(Player player, StateMachine<Player> stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
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

        if(entity.rb.linearVelocityY < 0)
        {
            stateMachine.ChangeState(entity.FallState);
        }

        if(entity.stateTimer <= 0 && entity.input.Player.Jump.WasPressedThisFrame())
        {
            stateMachine.ChangeState(entity.JumpState);
        }

        if((entity.stateTimer <= 0 && entity.input.Player.Roll.WasPressedThisFrame()) && entity.groundDetected)
        {
            stateMachine.ChangeState(entity.RollState);
        }

        if((entity.stateTimer <= 0 && entity.input.Player.Dash.WasPressedThisFrame()))
        {
            stateMachine.ChangeState(entity.DashState);
        }

        if(entity.input.Player.Attack.WasPressedThisFrame())
        {
            stateMachine.ChangeState(entity.AttackState);
        }

    }
}
