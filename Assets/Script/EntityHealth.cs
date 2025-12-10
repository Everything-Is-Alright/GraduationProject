using UnityEngine;
using UnityEngine.UI;

public class EntityHealth : MonoBehaviour, IDamgable
{
    private Slider healthBar;
    private EntityVFX entityvfx;
    private float maxHp = 100;
    protected float currentHp = 100;
    [SerializeField] public bool isDead;

    protected virtual void Awake()
    {
        entityvfx = GetComponent<EntityVFX>();
        healthBar = GetComponent<Slider>();
        currentHp = maxHp;
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

        healthBar.value = currentHp / maxHp;
    }
    protected virtual void Die()
    {
        isDead = true;
    }
}
