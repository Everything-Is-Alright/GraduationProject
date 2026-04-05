using UnityEngine;

/// <summary>
/// 简单导弹控制器：初始化时传入所属者与配置并指定朝向（1 或 -1）或指定方向向量
/// 行为：
/// - 在 Init 时根据朝向修正外观（localScale.x）或接受方向向量（directional）
/// - 每 FixedUpdate 设置速度（Rigidbody2D）
/// - 生命周期结束后自动销毁
/// - OnTriggerEnter2D 碰撞到可被伤害对象（实现 IDamgable）时造成伤害并销毁（仅当启用碰撞时）
/// </summary>
[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Missile1Controller : MonoBehaviour
{
    private Missile1Config config;
    private Transform owner; // 发射者，用于避免击中自己
    private int facing = 1; // 1 = 朝右, -1 = 朝左

    private Rigidbody2D rb;
    private Collider2D col;
    private float lifeTimer;

    // 方向性发射支持
    private bool useDirectional = false;
    private Vector2 directionNormalized = Vector2.right;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        // 导弹通常用触发器检测命中
        if (col != null) col.isTrigger = true;
    }

    private void FixedUpdate()
    {
        if (config == null || rb == null) return;

        if (useDirectional)
        {
            rb.linearVelocity = directionNormalized * config.horizontalSpeed;
        }
        else
        {
            rb.linearVelocity = new Vector2(config.horizontalSpeed * facing, rb.linearVelocity.y);
        }
    }

    private void Update()
    {
        if (config == null) return;
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 初始化导弹（在 Spawn 时调用）
    /// 支持两种模式：
    /// - 传入 direction （非 null）则使用方向向量（directional），忽略 facing 的水平行为
    /// - direction 为 null 则走原水平速度逻辑（根据 facing）
    /// </summary>
    /// <param name="owner">发射者 Transform（用于避免自伤）</param>
    /// <param name="config">Missile 配置</param>
    /// <param name="facingDirection">朝向：1 或 -1。若传 0 则视为 1。</param>
    /// <param name="direction">可选：方向向量（世界坐标方向），若提供则以此方向发射并忽略 facing 的水平逻辑</param>
    public void Init(Transform owner, Missile1Config config, int facingDirection = 1, Vector2? direction = null)
    {
        this.owner = owner;
        this.config = config;
        this.facing = facingDirection >= 0 ? 1 : -1;

        lifeTimer = Mathf.Max(0f, config.lifeTime);

        // 若提供方向向量，则启用方向性发射
        if (direction.HasValue)
        {
            useDirectional = true;
            directionNormalized = direction.Value.normalized;
            // 修正外观：若是纯水平项目仍希望朝向朝向x符号，则调整 localScale.x
            Vector3 ls = transform.localScale;
            if (Mathf.Abs(directionNormalized.x) > 0.01f)
                ls.x = Mathf.Abs(ls.x) * (directionNormalized.x >= 0 ? 1 : -1);
            transform.localScale = ls;
        }
        else
        {
            useDirectional = false;
            // 修正朝向：保持 prefab 在发射时与 Boss 同方向
            Vector3 ls = transform.localScale;
            ls.x = Mathf.Abs(ls.x) * this.facing;
            transform.localScale = ls;
        }

        // 立即设置速度，FixedUpdate 会持续保持
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            if (useDirectional)
                rb.linearVelocity = directionNormalized * config.horizontalSpeed;
            else
                rb.linearVelocity = new Vector2(config.horizontalSpeed * this.facing, rb.linearVelocity.y);
        }

        // 启用碰撞器（如果曾被禁用）
        if (col == null) col = GetComponent<Collider2D>();
        if (col != null) col.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == null) return;

        // 不要击中发射者或其子对象
        if (owner != null && (other.transform == owner || other.transform.IsChildOf(owner)))
            return;

        // 对目标造成伤害（项目中约定的接口）
        var damagable = other.GetComponent<IDamgable>();
        if (damagable != null && config != null)
        {
            damagable.TakeDamage(config.damage, 0f, owner != null ? owner : transform);
            // 可在此播放命中 VFX / 音效（若有）
            Destroy(gameObject);
            return;
        }

        // 若不是可被伤害对象，也可以选择撞击即销毁（根据需求）
        // Destroy(gameObject);
    }

    private void OnDisable()
    {
        // 清理速度，避免回收后残留
        if (rb != null) rb.linearVelocity = Vector2.zero;
    }
}
