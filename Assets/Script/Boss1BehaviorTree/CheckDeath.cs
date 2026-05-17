using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class CheckDeath : Conditional
{
    public SharedGameObject boss;

    public override TaskStatus OnUpdate()
    {
        // 校验Boss目标对象是否存在
        if (boss == null || boss.Value == null)
        {
            return TaskStatus.Failure;
        }

        // 获取Boss身上的血量组件
        EntityHealth healthComponent = boss.Value.GetComponent<EntityHealth>();
        // 组件不存在 或 血量大于0 → 未死亡
        if (healthComponent == null || healthComponent.currentHp > 0)
        {
            return TaskStatus.Failure;
        }

        // 血量≤0 → 已死亡
        return TaskStatus.Success;
    }
}