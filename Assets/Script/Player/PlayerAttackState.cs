using UnityEngine;

public class PlayerAttackState : EntityState<Player>
{
    private int comboIndex = 1;

    private float lastTimeAttack;

    private bool comboAttackQueued;

    public PlayerAttackState(Player player, StateMachine<Player> stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        comboAttackQueued = false;
        if (Time.time > lastTimeAttack + entity.comboResetTime || comboIndex > 2)
        {
            comboIndex = 1;
        }

        if (entity.moveInput.x != entity.playerFacing)
        {
            entity.HandleFlip(entity.moveInput.x);
        }

        entity.anim.SetInteger("BasicAttackIndex", comboIndex);
    }

    public override void Exit()
    {
        base.Exit();

        comboIndex++;

        lastTimeAttack = Time.time;
    }

    public override void Update()
    {
        base.Update();

        entity.SetVelocity(0, entity.rb.linearVelocityY);

        if(entity.input.Player.Attack.WasPressedThisFrame())
        {
            comboAttackQueued = true;
        }

        if (triggerCalled)
        {
            if(comboAttackQueued)
            {
                entity.anim.SetBool(animBoolName, false);
                entity.EnterAttackStateWithDelay();
            }
            else
            {   
                stateMachine.ChangeState(entity.IdleState);
            }
              
        }

    }
}
