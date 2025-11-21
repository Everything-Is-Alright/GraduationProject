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

        entity.stateTimer = rollDuration;

        entity.HandleFlip(entity.playerDir.x);
    }

    public override void Exit()
    {
        base.Exit();

        entity.SetVelocity(0, entity.rb.linearVelocityY);
        entity.stateTimer = 0f;
    }

    public override void Update()
    {
        base.Update();

        entity.SetVelocity(entity.playerFacing * entity.movespeed * entity.RollMoveMultiplier, entity.rb.linearVelocityY);

        entity.stateTimer -= Time.deltaTime;
        
        if(entity.stateTimer <= 0f)
        {
            stateMachine.ChangeState(entity.IdleState);
        }
    }
}
