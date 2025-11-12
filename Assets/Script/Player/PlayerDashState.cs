using UnityEngine;

public class PlayerDashState : EntityState
{
    public PlayerDashState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    private float dashDuration = 0.25f;

    public override void Enter()
    {
        base.Enter();

        if(player.moveInput.x != 0)
        {
            player.playerDir = player.moveInput;
        }
        else
        {
            player.playerDir = player.facingRight ? Vector2.right : Vector2.left;
        }

        player.stateTimer = dashDuration;
    }

    public override void Exit()
    {
        base.Exit();

        player.stateTimer = 0;
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(player.playerDir.x * player.movespeed * player.DashMoveMultiplier, 0);

        player.stateTimer -= Time.deltaTime;

        if(player.stateTimer <=0 )
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }
}
