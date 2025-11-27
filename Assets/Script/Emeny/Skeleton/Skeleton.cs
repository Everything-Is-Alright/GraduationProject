using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Accessibility;
using static UnityEngine.EventSystems.EventTrigger;

public class Skeleton : Entity<Skeleton>
{
    private CapsuleCollider2D capsuleCollider;

    [Header("Movement details")]
    [SerializeField] public float moveSpeed;

    [Header("Collision detail")]
    public bool cliffCheck;
    [SerializeField] private float cliffDistance;
    private Vector2 cliffCheckPosition;

    [Header("Battle details")]
    public float battleMoveSpeed = 3;
    public float attackDistance = 2;

    [Header("Player detection")]
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private float playerCheckDistance;

    public SkeletonIdleState IdleState {  get; private set; }
    public SkeletonWalkState WalkState {  get; private set; }
    public SkeletonAttackState AttackState { get; private set; }
    public SkeletonBattleState BattleState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new StateMachine<Skeleton>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();

        IdleState = new SkeletonIdleState(this, stateMachine, "IsIdle");
        WalkState = new SkeletonWalkState(this, stateMachine, "IsWalk");
        AttackState = new SkeletonAttackState(this, stateMachine, "IsAttack");
        BattleState = new SkeletonBattleState(this, stateMachine, "IsBattle");
    }

    private void Start()
    {        
        stateMachine.Initialize(IdleState);
    }
    private void Update()
    {
        HandleCollisionDetection();

        stateMachine.UpdateActiveState();
        anim.SetFloat("xVelocity", rb.linearVelocityX);
    }

    private void OnDrawGizmos()
    {
        CliffCheck();
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDistance, 0));
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(entityFacing * wallCheckDistance, 0, 0));
        Gizmos.DrawLine(cliffCheckPosition, cliffCheckPosition + new Vector2(0, -cliffDistance));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + entityFacing * playerCheckDistance, transform.position.y));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + entityFacing * attackDistance, transform.position.y));
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

    public RaycastHit2D PlayerDetection()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * entityFacing, playerCheckDistance, whatIsPlayer | whatIsGround);

        if (hit.collider == null || hit.collider.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            return default;
        }

        return hit;
    }
}
