using UnityEngine;

public class PlayerAttackState : EntityState
{
    public PlayerAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
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

        player.SetVelocity(0,player.rb.linearVelocityY);


        if (triggerCalled)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }
}
