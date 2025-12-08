using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

public class EntityVFX : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("Damage VFX")]
    [SerializeField] private Material onDamageMaterial;
    [SerializeField] private float onDamageDuration = .2f;
    private Material originalMaterial;
    private Coroutine onDamageVfxCoroutine;
    [SerializeField]private GameObject hitVfx;

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMaterial = sr.material;
    }

    public void CreateOnHitVFX(Transform target)
    {
        Instantiate(hitVfx, target.position, Quaternion.identity);
    }

    public void PlayerOnDamageVfx()
    {
        if (onDamageVfxCoroutine != null)
        {
            StopCoroutine(onDamageVfxCoroutine);
        }

        onDamageVfxCoroutine = StartCoroutine(OnDamageVfxCo());
    }

    private IEnumerator OnDamageVfxCo()
    {
        Debug.Log("RedVFX");
        sr.material = onDamageMaterial;
        yield return new WaitForSeconds(onDamageDuration);
        Debug.Log("NoneVFX");
        sr.material = originalMaterial;
    }
}
