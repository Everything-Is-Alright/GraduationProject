using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

// 多阶段攻击节点（前摇 -> 主攻阶段）：播放一次前摇（windup），进入主攻态后计时 attackDurationSeconds 秒完成。
// 逻辑参照 DoAttack1：设置动画参数启动播放，等待阶段完成才返回 Success；若参数被外部取消或超时则返回 Failure；攻击完成后触发冷却。
public class DoAttack2 : Action
{
    public SharedGameObject player;
    private Animator anim;
    private int animLayer = 0;

    // 动画状态名（请在 Animator 中按需修改）
    public string windupStateName = "Attack2Start";
    public string mainAttackStateName = "Attack2State";

    // 控制动画的参数名（Animator 中应有对应参数）
    public string attackParamName = "IsAttack2";

    // 使用枚举标识攻击类型（对应 AttackCooldown 中的配置）
    public AttackType attackType = AttackType.Attack2;
    // 可选：覆盖配置的冷却秒数（<=0 时使用 AttackCooldown 中 Inspector 的配置）
    public float overrideCooldownSeconds = 0f;

    // 主攻击阶段持续时间（秒）
    public float attackDurationSeconds = 5f;

    // 等待进入任一攻击阶段的超时保护（秒）
    public float waitToEnterTimeout = 0.5f;
    private float waitTimer;

    // 主攻击计时器
    private float attackTimer = 0f;
    // 标志：是否已经完成整个攻击流程，用于触发冷却
    private bool attackCompleted = false;

    private AttackCooldown cooldownComp;

    public override void OnAwake()
    {
        anim = gameObject.GetComponentInChildren<Animator>();
        cooldownComp = gameObject.GetComponent<AttackCooldown>();
    }

    public override void OnStart()
    {
        waitTimer = 0f;
        attackTimer = 0f;
        attackCompleted = false;

        if (anim != null)
        {
            anim.SetBool(attackParamName, true);
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (anim == null)
            return TaskStatus.Failure;

        var current = anim.GetCurrentAnimatorStateInfo(animLayer);

        // 处于主攻击态：按固定时间计时 attackDurationSeconds，计时完成则成功
        if (current.IsName(mainAttackStateName))
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackDurationSeconds)
            {
                attackCompleted = true;
                return TaskStatus.Success;
            }
            return TaskStatus.Running;
        }

        // 处于前摇态：等待前摇播放一次完成（normalizedTime >= 1）
        if (current.IsName(windupStateName))
        {
            // 播放完成后通常会过渡到主攻态，由过渡逻辑处理
            return TaskStatus.Running;
        }

        // 处于过渡中且即将进入前摇或主攻态：继续等待
        if (anim.IsInTransition(animLayer))
        {
            var next = anim.GetNextAnimatorStateInfo(animLayer);
            if (next.IsName(mainAttackStateName) || next.IsName(windupStateName))
                return TaskStatus.Running;
        }

        // 若攻击参数被外部置为 false，认为攻击被取消 -> Failure
        if (!anim.GetBool(attackParamName))
            return TaskStatus.Failure;

        // 等待进入任一攻击相关状态（windup 或 直接进入主攻击）
        waitTimer += Time.deltaTime;
        if (waitTimer <= waitToEnterTimeout)
        {
            return TaskStatus.Running;
        }

        // 超时仍未进入攻击相关状态 -> Failure（避免树卡住）
        Debug.LogWarning($"{gameObject.name}: DoAttack2 timed out waiting to enter attack states ('{windupStateName}' or '{mainAttackStateName}').");
        return TaskStatus.Failure;
    }

    public override void OnEnd()
    {
        // 结束动作（成功或被中断）时，重置参数并在成功完成时启动冷却
        if (anim != null)
            anim.SetBool(attackParamName, false);

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