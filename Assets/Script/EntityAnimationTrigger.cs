using System;
using UnityEngine;
using UnityEngine.Events;

public class EntityAnimationTrigger : MonoBehaviour
{
    private IEntity entity;
    private EntityCombat entityCombat;

    [Header("Animation Event UnityEvents (可在 Inspector 关联方法)")]
    public UnityEvent OnFireFrame;               // 原有通用事件（保留）
    public UnityEvent<int> OnFireInt;
    public UnityEvent<float> OnFireFloat;
    public UnityEvent<string> OnFireString;

    [Header("Boss专属独立攻击触发（隔离互不干扰）")]
    public UnityEvent OnAttack1;  // 对应：SpawnAtSpawnPoint_NoParam()
    public UnityEvent OnAttack2;  // 对应：SpawnAttack2_FiveUp_NoParam()
    public UnityEvent OnAttack3;  // 对应：SpawnAttack3_RandomPattern_NoParam()

    [Header("翻滚无敌触发")]
    public UnityEvent OnRollStart;  // 翻滚动画第一帧调用
    public UnityEvent OnRollEnd;    // 翻滚动画最后一帧调用

    private void Awake()
    {
        entity = GetComponentInParent<IEntity>();
        entityCombat = GetComponentInParent<EntityCombat>();
    }

    // 现有事件：通知实体当前状态触发（保留）
    public void CurrentStateTrigger()
    {
        entity.CallAnimationTrigger();
    }

    // 现有事件：执行攻击（保留）
    public void AttackTrigger()
    {
        entityCombat.PerformAttack();
    }

    // 现有事件：播放攻击音效（保留）
    public void AttackAudioTrigger()
    {
        AudioManager.Instance.PlayAttackSound();
    }

    // 现有事件：实体销毁（保留）
    public void EntityDestroyTrigger()
    {
        entity.EntityDestroy();
    }

    // 原有通用发射事件（保留）
    public void FireFrameTrigger()
    {
        OnFireFrame?.Invoke();
    }
    public void FireFrameTrigger_Int(int value)
    {
        OnFireInt?.Invoke(value);
    }
    public void FireFrameTrigger_Float(float value)
    {
        OnFireFloat?.Invoke(value);
    }
    public void FireFrameTrigger_String(string value)
    {
        OnFireString?.Invoke(value);
    }

    // ====================== 【新增】三个独立攻击触发方法 ======================
    /// <summary>
    /// 动画事件绑定：仅触发 Attack1（普通导弹）
    /// </summary>
    public void TriggerAttack1()
    {
        OnAttack1?.Invoke();
    }

    /// <summary>
    /// 动画事件绑定：仅触发 Attack2（向上导弹）
    /// </summary>
    public void TriggerAttack2()
    {
        OnAttack2?.Invoke();
    }

    /// <summary>
    /// 动画事件绑定：仅触发 Attack3（随机阵列）
    /// </summary>
    public void TriggerAttack3()
    {
        OnAttack3?.Invoke();
    }

    /// <summary>
    /// 动画事件：翻滚开始
    /// </summary>
    public void TriggerRollStart()
    {
        OnRollStart?.Invoke();
        // 子物体 → 直接修改父物体(Player)为无敌层级
        if (transform.parent != null)
            transform.parent.gameObject.layer = LayerMask.NameToLayer("PlayerInvincible");
    }

    public void TriggerRollEnd()
    {
        OnRollEnd?.Invoke();
        // 子物体 → 直接修改父物体(Player)恢复正常层级
        if (transform.parent != null)
            transform.parent.gameObject.layer = LayerMask.NameToLayer("Player");
    }
}