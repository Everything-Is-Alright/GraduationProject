using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

// 仿照 DoAttack2：前摇 -> 攻击 两阶段（前摇状态名 LayerStart，攻击状态名 LayerState）
// 逻辑与 DoAttack2 保持一致
public class DoAttack4 : Action
{
    public SharedGameObject player;
    private Animator anim;
    private int animLayer = 0;

    // 在 Animator 中对应的状态名
    public string windupStateName = "LaserStart";
    public string mainAttackStateName = "LaserState";

    // 控制 Animator 的参数名
    public string attackParamName = "IsAttack4";

    // 与 AttackCooldown 关联
    public AttackType attackType = AttackType.Attack4;
    public float overrideCooldownSeconds = 0f;

    // 攻击持续时间
    public float attackDurationSeconds = 5f;

    // 等待进入动画状态的超时
    public float waitToEnterTimeout = 0.5f;
    private float waitTimer;

    // 计时器与完成标志
    private float attackTimer = 0f;
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

        // 如果处于主攻击态，计时达到持续时长 -> Success，否则继续运行
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

        // 如果处于前摇态，继续等待前摇完成
        if (current.IsName(windupStateName))
        {
            return TaskStatus.Running;
        }

        // 如果在从任意态过渡到攻击相关态，也视为运行中
        if (anim.IsInTransition(animLayer))
        {
            var next = anim.GetNextAnimatorStateInfo(animLayer);
            if (next.IsName(mainAttackStateName) || next.IsName(windupStateName))
                return TaskStatus.Running;
        }

        // 如果外部将控制参数关闭，视为失败
        if (!anim.GetBool(attackParamName))
            return TaskStatus.Failure;

        // 等待进入前摇或攻击态，超时则失败
        waitTimer += Time.deltaTime;
        if (waitTimer <= waitToEnterTimeout)
        {
            return TaskStatus.Running;
        }

        Debug.LogWarning($"{gameObject.name}: DoAttack4 timed out waiting to enter attack states ('{windupStateName}' or '{mainAttackStateName}').");
        return TaskStatus.Failure;
    }

    public override void OnEnd()
    {
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