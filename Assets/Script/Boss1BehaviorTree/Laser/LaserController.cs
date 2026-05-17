﻿using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class LaserController : MonoBehaviour
{
    private Transform owner;
    private float damage = 0f;
    private float lifeTimer = 1f;
    private Collider2D col;
    private bool activeDamage = false;

    private int facing = 1;
    private bool useDirectional = false;
    private Vector2 directionNormalized = Vector2.right;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        if (col != null) col.isTrigger = true;
    }

    // 统一由动画触发器调用
    public void SetColliderEnabled(bool isEnabled)
    {
        if (col != null)
            col.enabled = isEnabled;
    }

    public void Init(Transform owner, float damage, float durationSeconds, bool activeDamage, int facingDirection = 1, Vector2? direction = null)
    {
        this.owner = owner;
        this.damage = Mathf.Max(0f, damage);
        this.lifeTimer = Mathf.Max(0f, durationSeconds);
        this.activeDamage = activeDamage;
        this.facing = facingDirection >= 0 ? 1 : -1;

        if (direction.HasValue)
        {
            useDirectional = true;
            directionNormalized = direction.Value.normalized;
            Vector3 ls = transform.localScale;
            if (Mathf.Abs(directionNormalized.x) > 0.01f)
                ls.x = Mathf.Abs(ls.x) * (directionNormalized.x >= 0 ? 1 : -1);
            transform.localScale = ls;
        }
        else
        {
            useDirectional = false;
            Vector3 ls = transform.localScale;
            ls.x = Mathf.Abs(ls.x) * this.facing;
            transform.localScale = ls;
        }

        // 初始关闭碰撞，由动画控制
        if (col != null) col.enabled = false;
    }

    private void Update()
    {
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!activeDamage || other == null) return;
        if (owner != null && (other.transform == owner || other.transform.IsChildOf(owner))) return;

        var damagable = other.GetComponent<IDamgable>();
        if (damagable != null)
        {
            damagable.TakeDamage(damage, 0f, owner != null ? owner : transform);
        }
    }
}