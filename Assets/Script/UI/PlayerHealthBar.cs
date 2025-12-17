using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public Image redHealth;
    public Image whiteHealth;
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

    private void UpdateHealthBar()
    {
        redHealth.fillAmount = entityHealth.currentHp / entityStats.GetMaxHealth();
        Debug.Log(entityHealth.currentHp);
        if(updateCoroutine != null)
        {
            StopCoroutine(updateCoroutine);
        }

        updateCoroutine = StartCoroutine(UpdateHpDelay());
    }
    
    private IEnumerator UpdateHpDelay()
    {
        float delayLength = whiteHealth.fillAmount - redHealth.fillAmount;
        float elapsedTime = 0f;

        while (elapsedTime < delayTime && delayLength != 0)
        {
            elapsedTime += Time.deltaTime;
            whiteHealth.fillAmount = Mathf.Lerp(redHealth.fillAmount + delayLength, redHealth.fillAmount, elapsedTime);
            yield return null;
        }

        whiteHealth.fillAmount = redHealth.fillAmount;
        updateCoroutine = null;
    }
}
