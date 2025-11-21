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
        entity.SetVelocity(0, entity.rb.linearVelocityY);

        if(entity.groundDetected && touchedGround == false)
        {
            touchedGround = true;
            entity.anim.SetTrigger("IsJumpAttackTrigger");
        }

        if (triggerCalled && entity.groundDetected)
        {
            stateMachine.ChangeState(entity.IdleState);
        }
    }
}
