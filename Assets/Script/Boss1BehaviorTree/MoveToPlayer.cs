using UnityEngine;
using System.Collections;
using BehaviorDesigner.Runtime.Tasks;

// 完全符合导师要求：
// 移动节点 = 只负责逻辑 + 驱动Animator参数
// 动画切换完全交给 Animator Controller
public class MoveToPlayer : Action
{
    [Header("移动速度")]
    public float moveSpeed = 1f;
    [Header("停止距离")]
    public float stopDistance = 0.2f;

    private GameObject player;
    private Animator anim;

    public override void OnStart()
    {
        player = GameObject.FindWithTag("Player");
        anim = gameObject.GetComponentInChildren<Animator>();
    }

    public override TaskStatus OnUpdate()
    {
        if (player == null)
        {
            SetMovingParam(false);
            return TaskStatus.Failure;
        }

        // 距离判断
        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance > stopDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
            SetMovingParam(true);  // 告诉动画：我在移动
        }
        else
        {
            SetMovingParam(false); // 告诉动画：我停下了
            return TaskStatus.Failure; // 到达目标，返回 Failure 以便上层节点切换到攻击
        }

        // 持续执行，直到被上层节点中断或返回 Failure
        return TaskStatus.Running;
    }

    // 唯一作用：设置动画参数（解耦核心）
    private void SetMovingParam(bool isMoving)
    {
        if (anim != null)
            anim.SetBool("IsMoving", isMoving);
    }

    // 节点中断（攻击）时，重置参数
    public override void OnEnd()
    {
        SetMovingParam(false);
    }
}