using Unity.VisualScripting;
using UnityEngine;

public class PlayerWallSlideState : EntityState<Player>
{
    public PlayerWallSlideState(Player player, StateMachine<Player> stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
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

        
        if(player.moveInput.y < 0 )
        {
            player.SetVelocity(0, player.rb.linearVelocityY );
        }
        else 
        {
            player.SetVelocity(0, player.rb.linearVelocityY * 0.6f);
        }

        if (player.groundDetected)
        {
            stateMachine.ChangeState(player.IdleState);
            player.HandleFlip(-player.playerFacing);
        }

        if(player.input.Player.Jump.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.WallJumpState);
        }

        if(!player.wallDetected)
        {
            stateMachine.ChangeState(player.FallState);
        }
    }
}
