using UnityEngine;

public class EntityCombat : MonoBehaviour
{

    [Header("Target detection")]
    [SerializeField] private Transform targetCheck;
    [SerializeField] private float targetCheckRadius;
    [SerializeField] private LayerMask whatIsTraget;
    private EntityStats stats;


    private EntityVFX vfx;

    public void Awake()
    {
        vfx = GetComponent<EntityVFX>();
        stats = GetComponent<EntityStats>();
    }
    public void PerformAttack()
    {
        foreach(var target in GetDetectedColliders())
        {
            IDamgable damgable = target.GetComponent<IDamgable>();
            
            
            damgable?.TakeDamage(stats.GetPhysicalDamage(), stats.GetMagicDamage(), transform);
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
