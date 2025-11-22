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

        
        if(entity.moveInput.y < 0 )
        {
            entity.SetVelocity(0, entity.rb.linearVelocityY );
        }
        else 
        {
            entity.SetVelocity(0, entity.rb.linearVelocityY * 0.6f);
        }

        if (entity.groundDetected)
        {
            stateMachine.ChangeState(entity.IdleState);
            entity.HandleFlip(-entity.entityFacing);
        }

        if(entity.input.Player.Jump.WasPressedThisFrame())
        {
            stateMachine.ChangeState(entity.WallJumpState);
        }

        if(!entity.wallDetected)
        {
            stateMachine.ChangeState(entity.FallState);
        }
    }
}
