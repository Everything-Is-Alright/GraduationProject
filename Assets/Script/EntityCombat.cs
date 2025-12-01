using UnityEngine;

public class EntityCombat : MonoBehaviour
{

    [Header("Target detection")]
    [SerializeField] private Transform targetCheck;
    [SerializeField] private float targetCheckRadius;
    [SerializeField] private LayerMask whatIsTraget;

    public float damage = 30;
    public void PerformAttack()
    {
        foreach(var target in GetDetectedColliders())
        {
            EntityHealth targetHealth = target.GetComponent<EntityHealth>();
            targetHealth?.TakeDamage(damage);
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
