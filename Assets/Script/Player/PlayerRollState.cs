using UnityEngine;

public class PlayerRollState : PlayerGroundState
{
    public PlayerRollState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        if(player.moveInput.x != 0)
        {
            player.RollDir = player.moveInput;
        }
        else
        {
            player.RollDir = player.facingRight ? Vector2.right : Vector2.left;
        }


        player.RollTimer = player.RollDuration;

        player.HandleFlip(player.RollDir.x);
    }

    public override void Exit()
    {
        base.Exit();

        player.SetVelocity(0, player.rb.linearVelocityY);
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(player.RollDir.x * player.movespeed * player.RollMoveMultiplier, player.rb.linearVelocityY);
        player.RollTimer -= Time.deltaTime;
        if(player.RollTimer <= 0f)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }
}
