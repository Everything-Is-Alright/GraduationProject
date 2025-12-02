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

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMaterial = sr.material;
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
        sr.material = onDamageMaterial;
        yield return new WaitForSeconds(onDamageDuration);
        sr.material = originalMaterial;
    }
}
