using UnityEngine;

public class PlayerJumpAttackState : EntityState<Player>
{
    public PlayerJumpAttackState(Player player, StateMachine<Player> stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    private bool touchedGround;

    public override void Enter()
    {
        base.Enter();
        touchedGround = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        player.SetVelocity(0, player.rb.linearVelocityY);

        if(player.groundDetected && touchedGround == false)
        {
            touchedGround = true;
            player.anim.SetTrigger("IsJumpAttackTrigger");
        }

        if (triggerCalled && player.groundDetected)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }
}
