using UnityEngine;

public interface IEntity
{
    Animator anim { get; }
    Rigidbody2D rb { get; }
}

public abstract class EntityState<T> where T : IEntity
{

    protected T entity;
    protected StateMachine<T> stateMachine;
    protected string animBoolName;
    protected bool triggerCalled;


    public EntityState(T entity,StateMachine<T> stateMachine, string animBoolName )
    {
        this.entity = entity;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }
    public void CallAnimationTrigger()
    {
        triggerCalled = true;
    }

    public virtual void Enter()
    {
        entity.anim.SetBool(animBoolName,true);
        triggerCalled = false;
    }


    public virtual void Update()
    {
        
    }

    public virtual void Exit()
    {
        entity.anim.SetBool(animBoolName,false);
    }
}
