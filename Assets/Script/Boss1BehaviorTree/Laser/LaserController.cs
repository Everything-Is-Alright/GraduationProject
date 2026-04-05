using UnityEngine;

/// <summary>
/// 激光控制器：可作为前摇（activeDamage=false）或激活态（activeDamage=true）
/// - 前摇态：通常禁用 Collider（或禁用触发伤害）用于视觉提示
/// - 激活态：启用触发器并对进入触发器的 IDamgable 造成伤害
/// - 生命周期结束后销毁
/// - 支持方向控制：可通过朝向或方向向量控制激光方向
/// - 【修复】激光生成后锁定朝向，不再随Boss朝向变化
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class LaserController : MonoBehaviour
{
    private Transform owner;
    private float damage = 0f;
    private float lifeTimer = 1f;
    private Collider2D col;
    private bool activeDamage = false;

    // 方向控制相关
    private int facing = 1; // 1 = 朝右, -1 = 朝左
    private bool useDirectional = false; // 是否使用方向向量
    private Vector2 directionNormalized = Vector2.right; // 归一化的方向向量

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        // 激光通常用触发器
        if (col != null) col.isTrigger = true;
    }

    /// <summary>
    /// 初始化激光
    /// 支持两种模式：
    /// - 传入 direction （非 null）则使用方向向量（directional），忽略 facing 的水平行为
    /// - direction 为 null 则走原水平方向逻辑（根据 facing）
    /// </summary>
    /// <param name="owner">发射者 Transform（用于避免自伤）</param>
    /// <param name="damage">激光伤害</param>
    /// <param name="durationSeconds">激光持续时间</param>
    /// <param name="activeDamage">是否激活伤害</param>
    /// <param name="facingDirection">朝向：1 或 -1。若传 0 则视为 1。</param>
    /// <param name="direction">可选：方向向量（世界坐标方向），若提供则以此方向发射并忽略 facing 的水平逻辑</param>
    public void Init(Transform owner, float damage, float durationSeconds, bool activeDamage, int facingDirection = 1, Vector2? direction = null)
    {
        this.owner = owner;
        this.damage = Mathf.Max(0f, damage);
        this.lifeTimer = Mathf.Max(0f, durationSeconds);
        this.activeDamage = activeDamage;
        this.facing = facingDirection >= 0 ? 1 : -1;

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
            // 初始化朝向
            Vector3 ls = transform.localScale;
            ls.x = Mathf.Abs(ls.x) * this.facing;
            transform.localScale = ls;
        }

        if (col != null)
        {
            col.enabled = activeDamage;
        }
    }

    private void Update()
    {
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!activeDamage || other == null) return;

        if (owner != null && (other.transform == owner || other.transform.IsChildOf(owner)))
            return;

        var damagable = other.GetComponent<IDamgable>();
        if (damagable != null)
        {
            damagable.TakeDamage(damage, 0f, owner != null ? owner : transform);
        }
    }
}