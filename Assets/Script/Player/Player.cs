using UnityEngine;

public class Player : MonoBehaviour 
{

    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public PlayerInputSet input { get; private set; }
    public StateMachine stateMachine;

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerFallState FallState { get; private set; }
    public PlayerRollState RollState { get; private set; }


    [Header("Movement details")]
    public float movespeed;
    public float jumpspeed = 5;
    public float InAirMoveMultuplier = .7f;
    public bool facingRight = true;
    [SerializeField]public float stateTimer;

    [Header("Roll detail")]
    public Vector2 RollDir;
    public float RollMoveMultiplier = 1.5f;

    [Header("collision detection")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    public bool groundDetected {  get; private set; }

    public Vector2 moveInput {  get; private set; }

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        stateMachine = new StateMachine();
        input = new PlayerInputSet();

        IdleState = new PlayerIdleState(this, stateMachine, "IsIdle");
        MoveState = new PlayerMoveState(this, stateMachine, "IsMove");
        JumpState = new PlayerJumpState(this, stateMachine, "IsJumpFall");
        FallState = new PlayerFallState(this, stateMachine, "IsJumpFall");
        RollState = new PlayerRollState(this, stateMachine, "IsRoll");
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
    private void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDistance, 0));
    }

    private void HandleCollisionDetection()
    {
        groundDetected = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }
}
