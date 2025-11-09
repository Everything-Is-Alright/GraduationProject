using UnityEngine;

public class PlayerGroundState : EntityState
{
    public PlayerGroundState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
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

        if(player.rb.linearVelocityY < 0)
        {
            stateMachine.ChangeState(player.FallState);
        }

        if(player.input.Player.Jump.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.JumpState);
        }
    }
}
