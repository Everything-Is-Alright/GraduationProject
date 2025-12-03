using JetBrains.Annotations;
using System.Collections;
using System.Diagnostics;
using System.Security.Cryptography;
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
    public float battleTimerDuration = 5;
    public float retreatDistance = 1;
    public Vector2 retreatVelocity;

    [Header("Player detection")]
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private float playerCheckDistance;

    public Transform player { get; private set; }

    public SkeletonIdleState IdleState {  get; private set; }
    public SkeletonWalkState WalkState {  get; private set; }
    public SkeletonAttackState AttackState { get; private set; }
    public SkeletonBattleState BattleState { get; private set; }
    public SkeletonDeathState DeathState { get; private set; }

    private Coroutine knockbackCo;
    private bool isKnocked;

    protected override void Awake()
    {
        base.Awake();

        capsuleCollider = GetComponent<CapsuleCollider2D>();

        IdleState = new SkeletonIdleState(this, stateMachine, "IsIdle");
        WalkState = new SkeletonWalkState(this, stateMachine, "IsWalk");
        AttackState = new SkeletonAttackState(this, stateMachine, "IsAttack");
        BattleState = new SkeletonBattleState(this, stateMachine, "IsBattle");
        DeathState = new SkeletonDeathState(this, stateMachine, "IsDie");
    }

    private void Start()
    {        
        stateMachine.Initialize(IdleState);
    }

    protected virtual void Update()
    {
        HandleCollisionDetection();

        stateMachine.UpdateActiveState();
        anim.SetFloat("xVelocity", rb.linearVelocityX);
    }

    public Transform GetPlayerRefence()
    {
        if (player == null)
        {
            player = PlayerDetection().transform;
        }

        return player; 
    }

    private void OnDrawGizmos()
    {
        CliffCheck();
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDistance, 0));
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(entityFacing * wallCheckDistance, 0, 0));
        Gizmos.DrawLine(cliffCheckPosition, cliffCheckPosition + new Vector2(0, -cliffDistance));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + entityFacing * playerCheckDistance, transform.position.y));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + entityFacing * attackDistance, transform.position.y));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + entityFacing * retreatDistance, transform.position.y));
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

    public void ReciveKnockback(Vector2 knockback, float duration)
    {
        if(knockbackCo != null)
        {
            StopCoroutine(knockbackCo);
        }

        knockbackCo = StartCoroutine(KnockbackCo(knockback, duration));
    }

    public void TryEnterBattleState(Transform player)
    {
        if (stateMachine.currentState ==  BattleState || stateMachine.currentState == AttackState)
        {
            return;
        }
        this.player = player;
        stateMachine.ChangeState(BattleState);
    }

    private IEnumerator KnockbackCo(Vector2 knockback, float duration)
    {
        isKnocked = true;
        rb.linearVelocity = knockback;

        yield return new WaitForSeconds(duration);

        rb.linearVelocity = Vector2.zero;
        isKnocked = false;
    }

    public override void SetVelocity(float xVelocity, float yVelocity)
    {
        if(isKnocked)
        {
            return; 
        }
        base.SetVelocity(xVelocity, yVelocity);
    }

    public override void EntityDeath()
    {
        base.EntityDeath();
        stateMachine.ChangeState(DeathState);
    }
}