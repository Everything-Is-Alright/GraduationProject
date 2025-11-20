using UnityEngine;

public class PlayerMoveState : PlayerGroundState
{
    public PlayerMoveState(Player player, StateMachine<Player> stateMachine, string stateName) : base(player, stateMachine, stateName)
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

        if (player.moveInput.x == 0 || player.wallDetected)
        {
            stateMachine.ChangeState(player.IdleState);
        }

        player.SetVelocity(player.moveInput.x * player.movespeed, player.rb.linearVelocityY);

        
    }
}
    
