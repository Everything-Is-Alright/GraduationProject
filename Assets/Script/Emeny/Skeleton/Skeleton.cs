using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Accessibility;

public class Skeleton : MonoBehaviour , IEntity
{
    public Animation anim;
    public Rigidbody rb;
    public StateMachine<Skeleton> stateMachine;

    public SkeletonIdleState IdleState {  get; private set; }

    public void Awake()
    {
        anim = new Animation();
        rb = new Rigidbody();
        stateMachine = new StateMachine<Skeleton>();

        IdleState = new SkeletonIdleState(this, );
    }
}
