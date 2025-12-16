using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    public Image redHp;
    public Image whiteHp;

    public EntityStats entityStats;
    public EntityHealth entityHp;
    private float delayTime = 0.5f;

    private Coroutine updateCoroutine;

    private void Awake()
    {
        entityStats = GetComponentInParent<EntityStats>();
        entityHp = GetComponentInParent<EntityHealth>();
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        //if (entityStats == null) { Debug.LogError("entityStatsÎª¿Õ", this); return; }
        redHp.fillAmount = entityHp.currentHp / entityStats.GetMaxHealth();

        if(updateCoroutine != null)
        {
            StopCoroutine(updateCoroutine);
        }

        updateCoroutine = StartCoroutine(UpdateHpDelay());
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
