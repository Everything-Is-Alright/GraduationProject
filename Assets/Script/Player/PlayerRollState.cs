using UnityEngine;

public class PlayerRollState : EntityState<Player>
{
    public PlayerRollState(Player player, StateMachine<Player> stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }
    private float rollDuration = 0.6f;

    public override void Enter()
    {
        base.Enter();

        player.stateTimer = rollDuration;

        player.HandleFlip(player.playerDir.x);
    }

    public override void Exit()
    {
        base.Exit();

        player.SetVelocity(0, player.rb.linearVelocityY);
        player.stateTimer = 0f;
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(player.playerFacing * player.movespeed * player.RollMoveMultiplier, player.rb.linearVelocityY);
        
        player.stateTimer -= Time.deltaTime;
        
        if(player.stateTimer <= 0f)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }
}
