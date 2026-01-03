using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class EnemyHealthBar : MonoBehaviour
{
    public Image redHp;
    public Image whiteHp;

    public EntityStats entityStats;
    public EntityHealth entityHealth;
    private float delayTime = 0.5f;

    private Coroutine updateCoroutine;

    private void Awake()
    {
        entityStats = GetComponentInParent<EntityStats>();
        entityHealth = GetComponentInParent<EntityHealth>();
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        //if (entityStats == null) { Debug.LogError("entityStatsÎª¿Õ", this); return; }
        redHp.fillAmount = entityHealth.currentHp / entityStats.GetMaxHealth();

        if(updateCoroutine != null)
        {
            StopCoroutine(updateCoroutine);
        }

        updateCoroutine = StartCoroutine(UpdateHpDelay());
        Debug.Log(entityHealth.currentHp);
    }

    private IEnumerator UpdateHpDelay()
    {
        float delayLength = whiteHp.fillAmount - redHp.fillAmount;
        float elapsedTime = 0f;

        while(elapsedTime < delayTime && delayLength != 0)
        {
            elapsedTime += Time.deltaTime;
            whiteHp.fillAmount = Mathf.Lerp(redHp.fillAmount + delayLength, redHp.fillAmount, elapsedTime / delayTime);
            yield return null;
        }

        whiteHp.fillAmount = redHp.fillAmount;
        updateCoroutine = null;
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
    }
}
