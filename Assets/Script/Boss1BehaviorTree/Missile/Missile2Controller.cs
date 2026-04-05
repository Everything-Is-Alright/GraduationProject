using BehaviorDesigner.Runtime.Tactical;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Missile2Controller : MonoBehaviour
{
    private Missile2Config config;
    private Transform owner;

    private Rigidbody2D rb;
    private Collider2D col;
    private float lifeTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        if (col != null) col.isTrigger = true;
    }

    private void FixedUpdate()
    {
        if (config == null || rb == null) return;
        // 固定向上飞行
        rb.linearVelocity = new Vector2(0, config.verticalSpeed);
    }

    private void Update()
    {
        if (config == null) return;
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f) Destroy(gameObject);
    }

    public void Init(Transform owner, Missile2Config config)
    {
        this.owner = owner;
        this.config = config;
        lifeTimer = Mathf.Max(0f, config.lifeTime);

        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = new Vector2(0, config.verticalSpeed);

        if (col == null) col = GetComponent<Collider2D>();
        if (col != null) col.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == null) return;

        // 防自伤（和原脚本一样）
        if (owner != null && (other.transform == owner || other.transform.IsChildOf(owner)))
            return;

        // 伤害系统（和原脚本完全一样）
        var damagable = other.GetComponent<IDamgable>();
        if (damagable != null && config != null)
        {
            damagable.TakeDamage(config.damage, 0f, owner != null ? owner : transform);
            Destroy(gameObject);
            return;
        }
    }

    private void OnDisable()
    {
        if (rb != null) rb.linearVelocity = Vector2.zero;
    }
}