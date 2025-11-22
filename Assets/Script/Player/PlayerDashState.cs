using UnityEngine;

public class PlayerDashState : EntityState<Player>
{
    public PlayerDashState(Player player, StateMachine<Player> stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    private float dashDuration = 0.25f;

    public override void Enter()
    {
        base.Enter();

        entity.stateTimer = dashDuration;
    }

    public override void Exit()
    {
        base.Exit();

        entity.stateTimer = 0;
    }

    public override void Update()
    {
        base.Update();

        if(entity.moveInput.x != 0)
        {
            entity.SetVelocity(entity.moveInput.x * entity.moveSpeed * entity.DashMoveMultiplier, 0);
        }
        else
        {
            entity.SetVelocity(entity.entityFacing * entity.moveSpeed * entity.DashMoveMultiplier, 0);
        }

        entity.stateTimer -= Time.deltaTime;

        if(entity.stateTimer <=0 )
        {
            stateMachine.ChangeState(entity.IdleState);
        }
    }
}
