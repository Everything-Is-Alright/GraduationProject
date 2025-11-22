using UnityEngine;

public class PlayerJumpState : PlayerAirState
{
    public PlayerJumpState(Player player, StateMachine<Player> stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        entity.SetVelocity(entity.rb.linearVelocityX, entity.jumpSpeed);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(entity.rb.linearVelocityY < 0)
        {
            stateMachine.ChangeState(entity.FallState);
        }
    }
}
