using UnityEngine;

public class EntityHealth : MonoBehaviour
{
    [SerializeField] protected float maxHp = 100;
    [SerializeField] public bool isDead;

    private EntityVFX entityvfx;
    protected virtual void Awake()
    {
        entityvfx = GetComponent<EntityVFX>();
    }

    public virtual void TakeDamage(float damage, Transform damageDealer)
    {
        if (isDead)
            return;

        entityvfx.PlayerOnDamageVfx();
        ReduceHp(damage);
    }

    protected void ReduceHp(float damage)
    {
        maxHp -= damage;

        if(maxHp <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        isDead = true;
    }
}
