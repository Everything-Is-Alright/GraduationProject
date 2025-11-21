using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Accessibility;

public class Skeleton : MonoBehaviour , IEntity
{
    public Animator anim {  get; private set; }
    public Rigidbody2D rb {  get; private set; }
    public StateMachine<Skeleton> stateMachine;

    [Header(" details")]
    [SerializeField] public float moveSpeed;

    public float stateTimer;

    public SkeletonIdleState IdleState {  get; private set; }
    public SkeletonWalkState WalkState {  get; private set; }

    public void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stateMachine = new StateMachine<Skeleton>();

        IdleState = new SkeletonIdleState(this, stateMachine, "IsIdle");
        WalkState = new SkeletonWalkState(this, stateMachine, "IsWalk");
    }

    public void Start()
    {        
        stateMachine.Initialize(IdleState);
    }
    public void Update()
    {
        stateMachine.UpdateActiveState();
    }

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        rb.linearVelocity = new Vector2 (xVelocity, yVelocity);
    }
}
