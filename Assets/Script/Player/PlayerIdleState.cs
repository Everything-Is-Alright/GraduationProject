using UnityEngine;

public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(Player player, StateMachine<Player> stateMachine, string stateName) : base(player,stateMachine, stateName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        entity.SetVelocity(0, entity.rb.linearVelocityY);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(entity.moveInput.x == entity.entityFacing && entity.wallDetected)
            return;

        if(entity.moveInput.x != 0 && entity.groundDetected)
        {
            stateMachine.ChangeState(entity.MoveState);
        }

    }
}
