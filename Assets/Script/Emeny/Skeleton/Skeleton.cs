using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Accessibility;

public class Skeleton : Entity
{
    private CapsuleCollider2D capsuleCollider;
    public StateMachine<Skeleton> stateMachine { get; private set; }

    [Header("Movement details")]
    [SerializeField] public float moveSpeed;

    [Header("Collision detail")]
    public bool cliffCheck;
    [SerializeField] private float cliffDistance;
    private Vector2 cliffCheckPosition;

    public SkeletonIdleState IdleState {  get; private set; }
    public SkeletonWalkState WalkState {  get; private set; }

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new StateMachine<Skeleton>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();

        IdleState = new SkeletonIdleState(this, stateMachine, "IsIdle");
        WalkState = new SkeletonWalkState(this, stateMachine, "IsWalk");
    }

    private void Start()
    {        
        stateMachine.Initialize(IdleState);
    }
    private void Update()
    {
        HandleCollisionDetection();

        stateMachine.UpdateActiveState();
    }

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        rb.linearVelocity = new Vector2 (xVelocity, yVelocity);
    }

    private void OnDrawGizmos()
    {
        CliffCheck();
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDistance, 0));
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(entityFacing * wallCheckDistance, 0, 0));
        Gizmos.DrawLine(cliffCheckPosition, cliffCheckPosition + new Vector2(0, -cliffDistance));
    }

    private void HandleCollisionDetection()
    {
        CliffCheck();
        groundDetected = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        wallDetected = Physics2D.Raycast(transform.position, Vector2.right * entityFacing, wallCheckDistance, whatIsGround);
        cliffCheck = Physics2D.Raycast(cliffCheckPosition, Vector2.down, cliffDistance, whatIsGround);
    }

    private void CliffCheck()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        float cliffCheckx = transform.position.x + (capsuleCollider.size.x / 2 * entityFacing);
        float cliffChecky = transform.position.y - capsuleCollider.size.y / 2;
        cliffCheckPosition = new Vector2(cliffCheckx,cliffChecky);
    }
}
