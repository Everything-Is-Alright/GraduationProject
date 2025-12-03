using System.Xml.Serialization;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public interface IEntity
{
    void CallAnimationTrigger();
}
public abstract class Entity<T> : MonoBehaviour, IEntity, IEntityState where T : Entity<T>
{
    public static Entity<T> instance { get; private set; }
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public StateMachine<T> stateMachine { get; set; }
    public float stateTimer;
    public bool facingRight = true;

    [Header("collision detection")]
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] public bool groundDetected;
    [SerializeField] public bool wallDetected;

    public int entityFacing = 1;

    protected virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stateMachine = new StateMachine<T>();
        instance = this;

    }
    void Start()
    {
    }

    void Update()
    {
    }

    public void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
        entityFacing = -entityFacing;
    }

    public virtual void SetVelocity(float xVelocity, float yVelocity)
    {
        rb.linearVelocity = new Vector2(xVelocity, yVelocity);
    }

    public void CallAnimationTrigger()
    {
        stateMachine.currentState.CallAnimationTrigger();
    }

    public void HandleFlip(float xVelocity)
    {
        if (xVelocity > 0 && facingRight == false)
        {
            Flip();
        }
        else if (xVelocity < 0 && facingRight == true)
        {
            Flip();
        }
    }
}
