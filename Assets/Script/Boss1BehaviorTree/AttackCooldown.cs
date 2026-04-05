using UnityEngine;
using System.Collections.Generic;
public enum AttackType
{
    Attack1,
    Attack2,
    Attack3,
    Attack4,
    Defense
}

[System.Serializable]
public struct AttackCooldownEntry
{
    public AttackType attack;
    public float cooldownSeconds;
}

public class AttackCooldown : MonoBehaviour
{
    [Header("在 Inspector 中为每种攻击设置冷却时间")]
    public AttackCooldownEntry[] cooldownEntries;

    [Header("是否使用不受 timeScale 影响的时间（例如暂停时仍计时）")]
    public bool useUnscaledTime = false;

    // 运行时记录每个攻击的剩余冷却时间（<=0 表示可用）
    private readonly Dictionary<AttackType, float> remaining = new Dictionary<AttackType, float>();
    // 配置映射（便于快速查询默认冷却）
    private readonly Dictionary<AttackType, float> configured = new Dictionary<AttackType, float>();

    void Awake()
    {
        // 初始化配置和运行时字典
        configured.Clear();
        remaining.Clear();
        foreach (var e in cooldownEntries)
        {
            configured[e.attack] = Mathf.Max(0f, e.cooldownSeconds);
            remaining[e.attack] = 0f;
        }

        // 确保所有枚举值都有条目（若 Inspector 未配置某些攻击）
        foreach (AttackType at in System.Enum.GetValues(typeof(AttackType)))
        {
            if (!configured.ContainsKey(at)) configured[at] = 0f;
            if (!remaining.ContainsKey(at)) remaining[at] = 0f;
        }
    }

    void Update()
    {
        float dt = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
        if (dt <= 0f) return;

        // 遍历减少剩余时间（注意不能在遍历中修改字典键集合）
        var keys = new List<AttackType>(remaining.Keys);
        foreach (var k in keys)
        {
            if (remaining[k] > 0f)
            {
                remaining[k] = Mathf.Max(0f, remaining[k] - dt);
            }
        }
    }

    // 启动指定攻击的冷却（使用 Inspector 中配置的时间）
    public void StartCooldown(AttackType attack)
    {
        float seconds = GetConfiguredCooldown(attack);
        if (seconds > 0f)
        {
            remaining[attack] = seconds;
        }
    }

    // 启动并指定自定义冷却时间（覆盖配置）
    public void StartCooldown(AttackType attack, float seconds)
    {
        if (seconds > 0f)
            remaining[attack] = seconds;
    }

    // 是否可以执行该攻击（true = 未冷却或冷却结束）
    public bool CanAttack(AttackType attack)
    {
        return !remaining.ContainsKey(attack) || remaining[attack] <= 0f;
    }

    // 获取剩余冷却时间（秒），不存在时返回 0
    public float GetRemaining(AttackType attack)
    {
        if (remaining.TryGetValue(attack, out var r))
            return r;
        return 0f;
    }

    // 获取 Inspector 中配置的默认冷却时间（不存在时返回 0）
    public float GetConfiguredCooldown(AttackType attack)
    {
        if (configured.TryGetValue(attack, out var c))
            return c;
        return 0f;
    }

    // 取消指定攻击的冷却（立即可用）
    public void CancelCooldown(AttackType attack)
    {
        if (remaining.ContainsKey(attack))
            remaining[attack] = 0f;
    }

    // 取消所有冷却
    public void CancelAll()
    {
        var keys = new List<AttackType>(remaining.Keys);
        foreach (var k in keys) remaining[k] = 0f;
    }

    // 调试用：是否正在冷却
    public bool IsCooling(AttackType attack) => !CanAttack(attack);
}