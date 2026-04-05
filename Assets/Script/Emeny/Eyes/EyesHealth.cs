using Unity.VisualScripting;
using UnityEngine;

public class EyesHealth : EntityHealth
{
    private Eyes eyes;

    [Header("Damage Knockback")]
    [SerializeField] private Vector2 onDamageKnockback;
    [SerializeField] private float knockbackDuration = 0.2f;

    protected override void Awake()
    {
        base.Awake();
        // 直接获取自己身上的Eyes脚本（多怪物安全）
        eyes = GetComponent<Eyes>();
    }

    private void Update()
    {
        if (Entity<Player>.instance != null)
            onDamageKnockback = new Vector2(3f * Entity<Player>.instance.entityFacing, 0);
    }

    public override void TakeDamage(float damage, float magicDamage, Transform damageDealer)
    {
        if (damageDealer.CompareTag("Player"))
        {
            eyes.TryEnterBattleState(damageDealer);
        }

        eyes.ReciveKnockback(onDamageKnockback, knockbackDuration);
        base.TakeDamage(damage, magicDamage, damageDealer);
    }

    protected override void Die()
    {
        base.Die();
        // 修复：调用自身的死亡方法，无单例，支持多怪物
        eyes.EntityDeath();
    }
}