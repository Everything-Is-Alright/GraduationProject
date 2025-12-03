using Unity.VisualScripting;
using UnityEngine;

public class SkeletonHealth : EntityHealth
{
    private Skeleton skeleton;
    [Header("Damage Knockback")]
    [SerializeField] private Vector2 onDamageKnockback;
    [SerializeField] private float knockbackDuration = 0.2f;

    protected override void Awake()
    {
        base.Awake();
        skeleton = GetComponent<Skeleton>(); 
    }

    private void Update()
    {
        onDamageKnockback = new Vector2(3f * Entity<Player>.instance.entityFacing, 0);
    }

    public override void TakeDamage(float damage, Transform damageDealer)
    {
        if(damageDealer.CompareTag("Player"))
        {
            skeleton.TryEnterBattleState(damageDealer);
        }

        skeleton.ReciveKnockback(onDamageKnockback, knockbackDuration);
        base.TakeDamage(damage, damageDealer);
    }

    protected override void Die()
    {
        base.Die();
        Entity<Skeleton>.instance.EntityDeath();
    }
}