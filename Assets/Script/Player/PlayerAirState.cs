using UnityEngine;

public class PlayerAirState : EntityState
{
    public PlayerAirState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
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

        if(player.moveInput.x != 0)
        {
            player.SetVelocity(player.moveInput.x * player.movespeed * player.InAirMoveMultuplier, player.rb.linearVelocityY);
        }

        if ((player.stateTimer <= 0 && player.input.Player.Dash.WasPressedThisFrame()))
        {
            stateMachine.ChangeState(player.DashState);
        }

        if(player.input.Player.Attack.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.JumpAttackState);
        }

    }
}
