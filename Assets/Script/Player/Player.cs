using System.Collections;
using UnityEngine;

public class Player : Entity
{
    public PlayerInputSet input { get; private set; }
    public StateMachine<Player> stateMachine { get; private set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerFallState FallState { get; private set; }
    public PlayerRollState RollState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerAttackState AttackState { get; private set; }
    public PlayerJumpAttackState JumpAttackState { get; private set; }


    [Header("Movement details")]
    public float moveSpeed;
    public float jumpSpeed = 12;
    public float InAirMoveMultuplier = .7f;
    public Vector2 wallJump;

    public Vector2 playerDir;


    [Header("Roll detail")]
    public float RollMoveMultiplier = 1.5f;

    [Header("Slide detail")]


    [Header("Dash detail")]
    public float DashMoveMultiplier = 2f;

    [Header("Attack detail")]
    public float comboResetTime = 1f;
    public Coroutine queuedAttackCo;

    

    public Vector2 moveInput {  get; private set; }

    protected override void Awake()
    {
        base.Awake();
        input = new PlayerInputSet();
        stateMachine = new StateMachine<Player>();

        IdleState = new PlayerIdleState(this, stateMachine, "IsIdle");
        MoveState = new PlayerMoveState(this, stateMachine, "IsMove");
        JumpState = new PlayerJumpState(this, stateMachine, "IsJumpFall");
        FallState = new PlayerFallState(this, stateMachine, "IsJumpFall");
        RollState = new PlayerRollState(this, stateMachine, "IsRoll");
        DashState = new PlayerDashState(this, stateMachine, "IsDash");
        WallSlideState = new PlayerWallSlideState(this, stateMachine, "IsWallSlide");
        WallJumpState = new PlayerWallJumpState(this, stateMachine, "IsJumpFall");
        AttackState = new PlayerAttackState(this, stateMachine, "IsAttack");
        JumpAttackState = new PlayerJumpAttackState(this, stateMachine, "IsJumpAttack");
    }

    private void OnEnable()
    {
        input.Enable();

        input.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Player.Movement.canceled += ctx => moveInput = Vector2.zero;
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
        rb.linearVelocity = new Vector2(xVelocity, yVelocity);
        HandleFlip(xVelocity);
    }

    public void HandleFlip(float xVelocity)
    {
        if(xVelocity > 0 && facingRight == false)
        {
            Flip();
        }
        else if(xVelocity < 0 && facingRight == true)
        {
            Flip();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDistance, 0));
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(entityFacing * wallCheckDistance, 0, 0));
    }

    

    public void CallAnimationTrigger()
    {
        stateMachine.currentState.CallAnimationTrigger();
    }

    public void EnterAttackStateWithDelay()
    {
        if(queuedAttackCo != null)
        {
            StopCoroutine(queuedAttackCo);
        }

        queuedAttackCo = StartCoroutine(EnterAttackStateWithDelayCo());
    }
    private IEnumerator EnterAttackStateWithDelayCo()
    {
        yield return new WaitForEndOfFrame();
        stateMachine.ChangeState(AttackState);
    }

    private void HandleCollisionDetection()
    {
        groundDetected = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        wallDetected = Physics2D.Raycast(transform.position, Vector2.right * entityFacing, wallCheckDistance, whatIsGround);
    }
}
