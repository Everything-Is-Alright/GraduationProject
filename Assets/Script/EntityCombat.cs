using UnityEngine;

public class EntityCombat : MonoBehaviour
{

    [Header("Target detection")]
    [SerializeField] private Transform targetCheck;
    [SerializeField] private float targetCheckRadius;
    [SerializeField] private LayerMask whatIsTraget;


    private EntityVFX vfx;
    public float damage = 30;

    public void Awake()
    {
        vfx = GetComponent<EntityVFX>();
    }
    public void PerformAttack()
    {
        foreach(var target in GetDetectedColliders())
        {
            IDamgable damgable = target.GetComponent<IDamgable>();
            
            
            damgable?.TakeDamage(damage, transform);
            vfx.CreateOnHitVFX(target.transform);
            //EntityHealth targetHealth = target.GetComponent<EntityHealth>();
            //targetHealth?.TakeDamage(damage, transform);
        }
    }
    private Collider2D[] GetDetectedColliders()
    {
        return Physics2D.OverlapCircleAll(targetCheck.position, targetCheckRadius, whatIsTraget);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetCheck.position, targetCheckRadius);
    }
}
