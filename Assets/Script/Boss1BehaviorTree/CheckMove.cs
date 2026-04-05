using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

// Condition：当玩家在 X 轴上位于 [minDistance, maxDistance] 区间内时返回 Success，否则返回 Failure。
// 支持使用黑板中的 SharedGameObject 作为目标，若未设置则回退到通过 Tag 查找 "Player"。
public class CheckMove : Conditional
{
    public SharedGameObject player;

    [Header("X轴最小距离（包含）")]
    public float minDistance = 0f;
    [Header("X轴最大距离（包含）")]
    public float maxDistance = 10f;

    public override TaskStatus OnUpdate()
    {
        // 优先使用黑板中的 SharedGameObject，再回退到 Tag 查找
        GameObject target = (player != null && player.Value != null) ? player.Value : GameObject.FindWithTag("Player");
        if (target == null)
        {
            return TaskStatus.Failure;
        }

        float xDiff = Mathf.Abs(target.transform.position.x - transform.position.x);

        // 保证 min <= max
        float min = Mathf.Min(minDistance, maxDistance);
        float max = Mathf.Max(minDistance, maxDistance);

        return xDiff >= min && xDiff <= max ? TaskStatus.Success : TaskStatus.Failure;
    }
}