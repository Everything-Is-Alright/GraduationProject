using System.Collections;
using UnityEngine;

public class Eyes : Entity<Eyes>
{
    [Header("Collision detail")]
    private Vector2 wallCheckPosition;

    [Header("Battle details")]
    public float battleMoveSpeed = 5f;
    public float attackDistance = 2f;
    public float battleTimerDuration = 5f;
    public float retreatDistance = 1f;
    public Vector2 retreatVelocity;

    [Header("Player detection")]
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private float playerCheckDistance;

    public Transform player { get; private set; }

    public EyesIdleState IdleState { get; private set; }
    public EyesFlyState FlyState { get; private set; }
    public EyesAttack1State Attack1State { get; private set; }
    public EyesAttack2State Attack2State { get; private set; }
    public EyesDieState DieState { get; private set; }

    private Coroutine knockbackCo;
    private bool isKnocked;
    public bool isWallDetected;

    protected override void Awake()
    {
        base.Awake();

        IdleState = new EyesIdleState(this, stateMachine, "IsIdle");
        FlyState = new EyesFlyState(this, stateMachine, "IsFly");
        Attack1State = new EyesAttack1State(this, stateMachine, "IsAttack1");
        Attack2State = new EyesAttack2State(this, stateMachine, "IsAttack2");
        DieState = new EyesDieState(this, stateMachine, "IsDie");
    }

    private void OnEnable()
    {
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.OnPlayerRespawn += OnPlayerRespawn;
        }
    }

    private void OnDisable()
    {
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.OnPlayerRespawn -= OnPlayerRespawn;
        }
        // 修复：关闭时停止协程，防止空引用
        if (knockbackCo != null)
            StopCoroutine(knockbackCo);
    }

    private void OnPlayerRespawn()
    {
        player = null;
        // 修复：玩家重生，回到待机状态（对齐Skeleton逻辑）
        stateMachine.ChangeState(IdleState);
    }

    private void Start()
    {
        stateMachine.Initialize(IdleState);
        // 飞行怪物：禁用重力（必须加，否则怪物会掉落）
        rb.gravityScale = 0;
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.UpdateActiveState();
        anim.SetFloat("xVelocity", rb.linearVelocityX);

        WallCheck();
    }

    public Transform GetPlayerRefence()
    {
        // 完全保留你原有逻辑，仅优化性能
        if (player == null || !player.gameObject.activeInHierarchy)
        {
            var hit = PlayerDetection();
            if (hit.collider != null)
                player = hit.transform;
            else
            {
                var playerObj = GameObject.FindWithTag("Player");
                if (playerObj != null)
                    player = playerObj.transform;
            }
        }
        return player;
    }

    public void ClearPlayerReference()
    {
        player = null;
    }
    private void OnDrawGizmos()
    {
        // 修复：移除未定义的 groundCheckDistance，避免报错
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(entityFacing * wallCheckDistance, 0, 0));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + entityFacing * playerCheckDistance, transform.position.y));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + entityFacing * attackDistance, transform.position.y));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + entityFacing * retreatDistance, transform.position.y));
    }

    private void WallCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * entityFacing, wallCheckDistance, whatIsGround);

        isWallDetected = hit.collider != null;
    }

    public RaycastHit2D PlayerDetection()
    {
        // 完全保留你原有逻辑
        for (int i = -1; i <= 1; i += 2)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * i, playerCheckDistance, whatIsPlayer);

            if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                return hit;
            }
        }
        return default;
    }

    public void TryEnterBattleState(Transform player)
    {
        if (stateMachine.currentState == Attack1State || stateMachine.currentState == Attack2State)
        {
            return;
        }
        this.player = player;
        stateMachine.ChangeState(FlyState);
    }

    // 保留你原有拼写：ReciveKnockback（不修改，避免报错）
    public void ReciveKnockback(Vector2 knockback, float duration)
    {
        if (knockbackCo != null)
        {
            StopCoroutine(knockbackCo);
        }
        knockbackCo = StartCoroutine(KnockbackCo(knockback, duration));
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
        if (isKnocked)
        {
            return;
        }
        base.SetVelocity(xVelocity, yVelocity);
    }

    public override void EntityDeath()
    {
        base.EntityDeath();
        stateMachine.ChangeState(DieState);
        rb.linearVelocity = Vector2.zero;
    }
}