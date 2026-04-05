using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class DoAttack1 : Action
{
    public SharedGameObject player;
    private Animator anim;
    private int animLayer = 0;

    // 可配置状态名，防止硬编码出错
    public string attackStateName = "Attack1";
    // 动画参数名（确保与 Animator 中一致）
    public string attackParamName = "IsAttack1";

    // 使用枚举标识攻击类型（对应 AttackCooldown 中的配置）
    public AttackType attackType = AttackType.Attack1;
    // 可选：覆盖配置的冷却秒数（<=0 时使用 AttackCooldown 中 Inspector 的配置）
    public float overrideCooldownSeconds = 0f;

    // 等待进入动画状态的超时时间（秒），防止一直等待导致行为树卡住
    public float waitToEnterTimeout = 0.5f;
    private float waitTimer;

    private AttackCooldown cooldownComp;
    private bool attackCompleted;

    public override void OnAwake()
    {
        anim = gameObject.GetComponentInChildren<Animator>();
        cooldownComp = gameObject.GetComponent<AttackCooldown>();
       
    }

    public override void OnStart()
    {
        waitTimer = 0f;
        attackCompleted = false;
        if (anim != null)
            anim.SetBool(attackParamName, true);
    }

    public override TaskStatus OnUpdate()
    {
        if (anim == null)
            return TaskStatus.Failure;

        var current = anim.GetCurrentAnimatorStateInfo(animLayer);

        // 已进入攻击状态：等待播放到接近结束再返回 Success
        if (current.IsName(attackStateName))
        {
            if (current.normalizedTime >= 0.95f)
            {
                attackCompleted = true;
                return TaskStatus.Success;
            }
            return TaskStatus.Running;
        }

        // 正在从其它状态过渡到 Attack1：继续等待
        if (anim.IsInTransition(animLayer))
        {
            var next = anim.GetNextAnimatorStateInfo(animLayer);
            if (next.IsName(attackStateName))
                return TaskStatus.Running;
        }

        // 如果攻击参数已被外部置为 false，认为攻击被取消 -> Failure
        if (!anim.GetBool(attackParamName))
            return TaskStatus.Failure;

        // 还未进入状态也未过渡：等待进入，但带超时保护
        waitTimer += Time.deltaTime;
        if (waitTimer <= waitToEnterTimeout)
        {
            return TaskStatus.Running;
        }

        // 超时仍未进入，返回 Failure 避免行为树卡住
        Debug.LogWarning($"{gameObject.name}: DoAttack1 timed out waiting to enter state '{attackStateName}'.");
        return TaskStatus.Failure;
    }

    public override void OnEnd()
    {
        // 动画结束或被打断 → 关闭参数，回到Idle
        if (anim != null)
            anim.SetBool(attackParamName, false);

        // 仅当攻击真正完成才开始冷却
        if (attackCompleted)
        {
            if (cooldownComp == null)
                cooldownComp = gameObject.GetComponent<AttackCooldown>();

            if (cooldownComp != null)
            {
                if (overrideCooldownSeconds > 0f)
                    cooldownComp.StartCooldown(attackType, overrideCooldownSeconds);
                else
                    cooldownComp.StartCooldown(attackType);
            }
        }
    }
}