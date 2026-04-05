using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

// 防御节点（前摇 -> 防御阶段 -> 后摇）：参考 DoAttack3 逻辑，完成后触发冷却。
public class DoDefense : Action
{
    public SharedGameObject player;
    private Animator anim;
    private int animLayer = 0;

    // 动画状态名（请在 Animator 中按需修改）
    public string windupStateName = "DefenseStart";
    public string mainStateName = "DefenseState";
    public string recoveryStateName = "DefenseEnd";

    // 控制动画的参数名（Animator 中应有对应参数）
    public string defenseParamName = "IsDefense";

    // 使用枚举标识（在 AttackCooldown 中配置）
    public AttackType attackType = AttackType.Defense;
    // 可选：覆盖配置的冷却秒数（<=0 时使用 AttackCooldown 中 Inspector 的配置）
    public float overrideCooldownSeconds = 0f;

    // 防御阶段持续时间（秒）
    public float defenseDurationSeconds = 2f;

    // 等待进入任一阶段的超时保护（秒）
    public float waitToEnterTimeout = 0.5f;
    private float waitTimer;

    // 主阶段计时器
    private float mainTimer = 0f;
    // 标志：是否完成整个防御流程（用于触发冷却）
    private bool actionCompleted = false;

    // 当主阶段结束但 Animator 没有后摇时的回退等待（秒）
    public float recoveryWaitTimeout = 0.25f;
    private float recoveryWaitTimer = 0f;
    private bool mainPhaseFinished = false;

    private AttackCooldown cooldownComp;

    public override void OnAwake()
    {
        anim = gameObject.GetComponentInChildren<Animator>();
        cooldownComp = gameObject.GetComponent<AttackCooldown>();
    }

    public override void OnStart()
    {
        waitTimer = 0f;
        mainTimer = 0f;
        recoveryWaitTimer = 0f;
        actionCompleted = false;
        mainPhaseFinished = false;

        if (anim != null)
            anim.SetBool(defenseParamName, true);
    }

    public override TaskStatus OnUpdate()
    {
        if (anim == null)
            return TaskStatus.Failure;

        var current = anim.GetCurrentAnimatorStateInfo(animLayer);

        // 如果处于后摇态：等待后摇接近结束 -> Success
        if (current.IsName(recoveryStateName))
        {
            if (current.normalizedTime >= 0.95f)
            {
                actionCompleted = true;
                return TaskStatus.Success;
            }
            return TaskStatus.Running;
        }

        // 主防御态：计时，计时结束后等待过渡到后摇或使用回退判断成功
        if (current.IsName(mainStateName))
        {
            mainTimer += Time.deltaTime;
            if (mainTimer >= defenseDurationSeconds)
            {
                mainPhaseFinished = true;
                // 如果 Animator 有后摇的过渡，保持 Running 并等待；否则使用回退等待计时成功
                if (anim.IsInTransition(animLayer))
                {
                    var next = anim.GetNextAnimatorStateInfo(animLayer);
                    if (next.IsName(recoveryStateName))
                        return TaskStatus.Running;
                }
                recoveryWaitTimer += Time.deltaTime;
                if (recoveryWaitTimer >= recoveryWaitTimeout)
                {
                    actionCompleted = true;
                    return TaskStatus.Success;
                }
                return TaskStatus.Running;
            }
            return TaskStatus.Running;
        }

        // 前摇：等待其播放并由 Animator 过渡到主阶段
        if (current.IsName(windupStateName))
        {
            return TaskStatus.Running;
        }

        // 过渡中且目标为防御相关状态：继续等待
        if (anim.IsInTransition(animLayer))
        {
            var next = anim.GetNextAnimatorStateInfo(animLayer);
            if (next.IsName(windupStateName) || next.IsName(mainStateName) || next.IsName(recoveryStateName))
                return TaskStatus.Running;
        }

        // 参数被取消 -> Failure
        if (!anim.GetBool(defenseParamName))
            return TaskStatus.Failure;

        // 等待进入任一防御相关状态
        waitTimer += Time.deltaTime;
        if (waitTimer <= waitToEnterTimeout)
            return TaskStatus.Running;

        Debug.LogWarning($"{gameObject.name}: DoDefense timed out waiting to enter defense states ('{windupStateName}', '{mainStateName}' or '{recoveryStateName}').");
        return TaskStatus.Failure;
    }

    public override void OnEnd()
    {
        if (anim != null)
            anim.SetBool(defenseParamName, false);

        if (actionCompleted)
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