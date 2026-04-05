using UnityEngine;

/// <summary>
/// 2D导弹生成器（适配Boss动画帧事件）
/// - 支持多发射点（攻击2/3）和激光生成
/// - 提供无参/Int参数方法，完美对接动画事件
/// - 生成的物体默认为场景根对象
/// </summary>
public class AttackSpawner : MonoBehaviour
{
    [Header("===== Missile1 (Attack1 专用) =====")]
    [Tooltip("导弹1预制体，必须包含 Missile1Controller 组件")]
    [SerializeField] private GameObject missilePrefab;
    [Tooltip("默认导弹1配置文件")]
    [SerializeField] private Missile1Config defaultConfig;
    [Tooltip("导弹发射点（为空则使用自身位置）")]
    [SerializeField] private Transform spawnPoint;
    [Tooltip("导弹父物体（仅用于场景收纳，可选）")]
    [SerializeField] private Transform missileParent;

    [Header("===== Missile2 (Attack2 专用向上导弹) =====")]
    [Tooltip("Attack2专用：竖直向上导弹预制体（Missile2）")]
    [SerializeField] private GameObject missile2Prefab;
    [Tooltip("Attack2专用：导弹2配置文件")]
    [SerializeField] private Missile2Config defaultMissile2Config;

    // ====================== Attack3 专属激光配置 ======================
    [Header("===== Laser3 (Attack3 专用随机激光) =====")]
    [Tooltip("Attack3专用激光预制体")]
    [SerializeField] private GameObject laser3Prefab;
    [Tooltip("Attack3专用激光配置（LaserConfig）")]
    [SerializeField] private LaserConfig laser3Config;

    [Header("===== Attack2 (五点向上) =====")]
    [Tooltip("攻击2 的 5 个发射点")]
    [SerializeField] private Transform[] attack2SpawnPoints = new Transform[0];

    [Header("===== Attack3 (两套随机激光阵列) ====")]
    [Tooltip("攻击3 第一套发射点")]
    [SerializeField] private Transform[] attack3PatternA = new Transform[0];
    [Tooltip("攻击3 第二套发射点")]
    [SerializeField] private Transform[] attack3PatternB = new Transform[0];

    // 缓存实体组件
    private IEntity entity;

    private void Awake()
    {
        entity = GetComponentInParent<IEntity>();
    }

    /// <summary>
    /// 通用生成（Missile1专用）
    /// </summary>
    public Missile1Controller Spawn(Vector2 position, int facing, Missile1Config config = null, Transform owner = null, Vector2? direction = null)
    {
        if (missilePrefab == null)
        {
            Debug.LogWarning("MissileSpawner: 未设置导弹1预制体！", this);
            return null;
        }

        config ??= defaultConfig;
        if (config == null)
        {
            Debug.LogWarning("MissileSpawner: 未设置导弹1配置文件！", this);
            return null;
        }

        GameObject missileGo = Instantiate(missilePrefab, position, Quaternion.identity);
        Missile1Controller controller = missileGo.GetComponent<Missile1Controller>();

        if (controller == null)
        {
            Debug.LogError("MissileSpawner: 导弹1预制体缺少 MissileController 组件！", this);
            Destroy(missileGo);
            return null;
        }

        controller.Init(owner ?? transform, config, facing, direction);
        return controller;
    }

    public Missile1Controller SpawnAtTransform(Transform spawnTransform, int facing, Missile1Config config = null, Transform owner = null, Vector2? direction = null)
    {
        Vector2 pos = spawnTransform != null ? (Vector2)spawnTransform.position : (Vector2)transform.position;
        return Spawn(pos, facing, config, owner, direction);
    }

    private Vector2 ResolveCustomSpawnPosition(Transform spawnTransform, int facing)
    {
        if (spawnTransform == null) return (Vector2)transform.position;
        if (spawnTransform.IsChildOf(transform)) return (Vector2)spawnTransform.position;

        Vector3 localOffset = spawnTransform.localPosition;
        Vector3 mirroredOffset = new Vector3(localOffset.x * facing, localOffset.y, localOffset.z);
        return (Vector2)transform.TransformPoint(mirroredOffset);
    }

    private Vector2 ResolveSpawnPosition(int facing)
    {
        if (spawnPoint == null)
            return (Vector2)transform.position;

        if (spawnPoint.IsChildOf(transform))
            return (Vector2)spawnPoint.position;

        Vector3 localOffset = spawnPoint.localPosition;
        Vector3 mirroredOffset = new Vector3(localOffset.x * facing, localOffset.y, localOffset.z);
        return (Vector2)transform.TransformPoint(mirroredOffset);
    }

    // -----------------------
    // 动画事件调用接口
    // -----------------------

    // Attack1 接口
    public void SpawnAtSpawnPoint_NoParam()
    {
        int facing = transform.localScale.x >= 0 ? 1 : -1;
        Vector2 spawnPos = ResolveSpawnPosition(facing);
        Spawn(spawnPos, facing);
    }

    public void SpawnAtSpawnPoint_Int(int facingValue)
    {
        int facing = facingValue >= 0 ? 1 : -1;
        Vector2 spawnPos = ResolveSpawnPosition(facing);
        Spawn(spawnPos, facing);
    }

    // ---------- Attack2: 五点向上导弹 ----------
    public void SpawnAttack2_FiveUp_NoParam()
    {
        int facing = transform.localScale.x >= 0 ? 1 : -1;
        SpawnAttack2_FiveUp_Internal(facing);
    }

    public void SpawnAttack2_FiveUp_Int(int facingValue)
    {
        int facing = facingValue >= 0 ? 1 : -1;
        SpawnAttack2_FiveUp_Internal(facing);
    }

    private void SpawnAttack2_FiveUp_Internal(int facing)
    {
        if (missile2Prefab == null)
        {
            Debug.LogError("【Attack2】未设置 Missile2 预制体！", this);
            return;
        }
        if (defaultMissile2Config == null)
        {
            Debug.LogError("【Attack2】未设置 Missile2 配置！", this);
            return;
        }

        if (attack2SpawnPoints == null || attack2SpawnPoints.Length == 0)
        {
            Vector2 pos = ResolveSpawnPosition(facing);
            SpawnMissile2At(pos);
            return;
        }

        foreach (var t in attack2SpawnPoints)
        {
            Vector2 pos = ResolveCustomSpawnPosition(t, facing);
            SpawnMissile2At(pos);
        }
    }

    private void SpawnMissile2At(Vector2 position)
    {
        GameObject missileGo = Instantiate(missile2Prefab, position, missile2Prefab.transform.rotation);
        Missile2Controller controller = missileGo.GetComponent<Missile2Controller>();

        if (controller == null)
        {
            Debug.LogError("【Attack2】缺少 Missile2Controller 组件！", this);
            Destroy(missileGo);
            return;
        }

        controller.Init(transform, defaultMissile2Config);
    }

    // ---------- Attack3: 随机激光阵列 ----------
    public void SpawnAttack3_RandomPattern_NoParam()
    {
        SpawnAttack3_RandomPattern_Internal();
    }

    public void SpawnAttack3_RandomPattern_Int(int dummy)
    {
        SpawnAttack3_RandomPattern_Internal();
    }

    private void SpawnAttack3_RandomPattern_Internal()
    {
        // 配置校验
        if (laser3Prefab == null)
        {
            Debug.LogError("【Attack3】未设置专用激光预制体！", this);
            return;
        }
        if (laser3Config == null)
        {
            Debug.LogError("【Attack3】未设置 LaserConfig 配置！", this);
            return;
        }

        // 随机选择发射组
        Transform[] chosenPattern = Random.value < 0.5f ? attack3PatternA : attack3PatternB;
        if (chosenPattern == null || chosenPattern.Length == 0)
        {
            Debug.LogWarning("【Attack3】无可用发射点！", this);
            return;
        }

        // 批量生成激光
        foreach (var point in chosenPattern)
        {
            if (point == null) continue;
            SpawnLaser3At(point.position);
        }
    }

    /// <summary>
    /// 生成Attack3激光（✅ 完全匹配LaserController参数）
    /// </summary>
    private void SpawnLaser3At(Vector2 position)
    {
        GameObject laserGo = Instantiate(laser3Prefab, position, laser3Prefab.transform.rotation);
        LaserController laser = laserGo.GetComponent<LaserController>();

        if (laser == null)
        {
            Debug.LogError("【Attack3】激光缺少 LaserController 组件！", this);
            Destroy(laserGo);
            return;
        }

        // 获取当前朝向（参考导弹1的处理方式）
        int facing = transform.localScale.x >= 0 ? 1 : -1;

        // 🔥 核心修正：完整传入参数，匹配 LaserController
        // owner | 伤害 | 生命周期 | 激活伤害(=true) | 朝向 | 方向向量
        laser.Init(transform, laser3Config.damage, laser3Config.lifeTime, true, facing);
    }
}