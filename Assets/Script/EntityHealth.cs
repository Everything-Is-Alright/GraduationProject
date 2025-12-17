using UnityEngine;
using UnityEngine.UI;

public class EntityHealth : MonoBehaviour, IDamgable
{
    private EntityVFX entityvfx;
    private EnemyHealthBar healthBar;
    public float currentHp;
    private EntityStats entityStats;
    [SerializeField] public bool isDead;

    protected virtual void Awake()
    {
        entityvfx = GetComponent<EntityVFX>();
        healthBar = GetComponentInChildren<EnemyHealthBar>();
        entityStats = GetComponent<EntityStats>();
        currentHp = entityStats.GetMaxHealth();
        UpdateHealthBar();
    }

    public virtual void TakeDamage(float damage, Transform damageDealer)
    {
        if (isDead)
            return;

        entityvfx.PlayerOnDamageVfx();
        ReduceHp(damage);
        Debug.Log(currentHp);
    }

    protected void ReduceHp(float damage)
    {
        currentHp -= damage;
        UpdateHealthBar();

        if(currentHp <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBar == null)
        {
            return;
        }
        healthBar.UpdateHealthBar();
    }

    protected virtual void Die()
    {
        isDead = true;
    }
}
