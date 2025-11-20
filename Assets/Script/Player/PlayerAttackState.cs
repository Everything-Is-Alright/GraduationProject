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
        if (Time.time > lastTimeAttack + player.comboResetTime || comboIndex > 2)
        {
            comboIndex = 1;
        }

        if (player.moveInput.x != player.playerFacing)
        {
            player.HandleFlip(player.moveInput.x);
        }

        player.anim.SetInteger("BasicAttackIndex", comboIndex);
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

        player.SetVelocity(0,player.rb.linearVelocityY);

        if(player.input.Player.Attack.WasPressedThisFrame())
        {
            comboAttackQueued = true;
        }

        if (triggerCalled)
        {
            if(comboAttackQueued)
            {
                player.anim.SetBool(animBoolName, false);
                player.EnterAttackStateWithDelay();
            }
            else
            {   
                stateMachine.ChangeState(player.IdleState);
            }
              
        }

    }
}
