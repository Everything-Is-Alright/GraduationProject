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

        if (entity.moveInput.x == 0 || entity.wallDetected)
        {
            stateMachine.ChangeState(entity.IdleState);
        }

        entity.SetVelocity(entity.moveInput.x * entity.moveSpeed, entity.rb.linearVelocityY);

        
    }
}
    
