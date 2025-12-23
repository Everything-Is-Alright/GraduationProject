using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class EntityHealth : MonoBehaviour, IDamgable
{
    private EntityVFX entityvfx;
    private EntityStats entityStats;
    private EnemyHealthBar enemyBar;
    private PlayerHealthBar playerBar;
    public float currentHp;
    [SerializeField] public bool isDead;

    protected virtual void Awake()
    {
        entityvfx = GetComponent<EntityVFX>();
        enemyBar = GetComponentInChildren<EnemyHealthBar>();
        playerBar = GetComponentInChildren<PlayerHealthBar>();
        entityStats = GetComponent<EntityStats>();
        currentHp = entityStats.GetMaxHealth();
        UpdateHealthBar();
        //Debug.Log("EntityHealth Awake÷¥––£¨currentHp≥ı ºªØ£∫" + currentHp);
    }

    public virtual void TakeDamage(float damage, Transform damageDealer)
    {
        if (isDead)
            return;

        float mitigation = entityStats.GetArmorMitigation();
        float finalDamage = damage * (1 -  mitigation);
        entityvfx.PlayerOnDamageVfx();
        ReduceHp(finalDamage);

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
        if(enemyBar != null || playerBar != null)
        {
            if(enemyBar != null)
            {
                enemyBar.UpdateHealthBar();
            }

            if(playerBar != null)
            {
                playerBar.UpdateHealthBar();
            }
        }
        else
        {
            return;
        }
        
    }

    protected virtual void Die()
    {
        isDead = true;
    }
}
