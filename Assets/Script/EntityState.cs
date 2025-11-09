using UnityEngine;

public abstract class EntityState
{

    protected Player player;
    protected StateMachine stateMachine;
    protected string animBoolName;
    
    protected Animator anim;


    public EntityState(Player player,StateMachine stateMachine, string animBoolName )
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        player.anim.SetBool(animBoolName,true);
    }

    public virtual void Update()
    {
        player.anim.SetFloat("yVelocity", player.rb.linearVelocity.y);
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName,false);
    }
}
