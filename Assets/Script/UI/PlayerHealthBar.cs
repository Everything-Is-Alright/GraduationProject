using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public Image redHealth;
    public Image whiteHealth;
    public EntityStats entityStats;
    public EntityHealth entityHp;
    private float delayTime = 0.5f;

    private Coroutine updateCoroutine;

    private void Awake()
    {
        entityStats = GetComponent<EntityStats>();
        entityHp = GetComponent<EntityHealth>();
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        redHealth.fillAmount = entityHp.currentHp / entityStats.GetMaxHealth();
        
        if(updateCoroutine != null)
        {
            StopCoroutine(updateCoroutine);
        }

        updateCoroutine = StartCoroutine(updatUpdateHpDelay());
    }
    


}
