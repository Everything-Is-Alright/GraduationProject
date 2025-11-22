using UnityEngine;

public class PlayerAirState : EntityState<Player>
{
    public PlayerAirState(Player player, StateMachine<Player> stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
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

        if(entity.moveInput.x != 0)
        {
            entity.SetVelocity(entity.moveInput.x * entity.moveSpeed * entity.InAirMoveMultuplier, entity.rb.linearVelocityY);
        }

        if ((entity.stateTimer <= 0 && entity.input.Player.Dash.WasPressedThisFrame()))
        {
            stateMachine.ChangeState(entity.DashState);
        }

        if(entity.input.Player.Attack.WasPressedThisFrame())
        {
            stateMachine.ChangeState(entity.JumpAttackState);
        }

    }
}
