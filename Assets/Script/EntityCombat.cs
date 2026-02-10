using UnityEngine;
using UnityEngine.Audio;

public class EntityCombat : MonoBehaviour
{

    [Header("Target detection")]
    [SerializeField] private Transform targetCheck;
    [SerializeField] private float targetCheckRadius;
    [SerializeField] private LayerMask whatIsTraget;
    private EntityStats stats;

    [Header("Attack Sound")]
    [SerializeField] private AudioClip attackSoundClip;
    private AudioSource audioSource;

    private EntityVFX vfx;

    public void Awake()
    {
        vfx = GetComponent<EntityVFX>();
        stats = GetComponent<EntityStats>();
        audioSource = GetComponent<AudioSource>();
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

    public void AttackFX()
    {
        audioSource.PlayOneShot(attackSoundClip);
        Debug.Log("触发了第一段攻击音效！");
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
