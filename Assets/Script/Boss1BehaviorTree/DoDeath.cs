using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class DoDeath : Action
{
    private Animator anim;
    private int animLayer = 0;

    // 死亡动画状态名
    public string deathStateName = "Death";
    // 死亡动画布尔参数名
    public string deathParamName = "IsDeath";

    // 等待进入动画状态的超时时间
    public float waitToEnterTimeout = 0.5f;
    private float waitTimer;

    public override void OnAwake()
    {
        // 获取子物体的动画控制器
        anim = gameObject.GetComponentInChildren<Animator>();
    }

    public override void OnStart()
    {
        waitTimer = 0f;
        // 启动时将死亡布尔参数设为true，触发死亡动画
        if (anim != null)
            anim.SetBool(deathParamName, true);
    }

    public override TaskStatus OnUpdate()
    {
        // 无动画组件直接返回失败
        if (anim == null)
            return TaskStatus.Failure;

        var current = anim.GetCurrentAnimatorStateInfo(animLayer);

        // 检测是否进入死亡动画状态，动画播放完成返回成功
        if (current.IsName(deathStateName))
        {
            if (current.normalizedTime >= 0.95f)
            {
                return TaskStatus.Success;
            }
            return TaskStatus.Running;
        }

        // 检测是否正在切换到死亡动画
        if (anim.IsInTransition(animLayer))
        {
            var next = anim.GetNextAnimatorStateInfo(animLayer);
            if (next.IsName(deathStateName))
                return TaskStatus.Running;
        }

        // 超时等待逻辑
        waitTimer += Time.deltaTime;
        if (waitTimer <= waitToEnterTimeout)
        {
            return TaskStatus.Running;
        }

        // 超时未进入死亡动画，返回失败
        Debug.LogWarning($"{gameObject.name}: DoDeath timed out waiting to enter state '{deathStateName}'.");
        return TaskStatus.Failure;
    }

}