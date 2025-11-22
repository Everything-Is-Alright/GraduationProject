using System.Xml.Serialization;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public abstract class Entity : MonoBehaviour , IEntity
{
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
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

    
}
