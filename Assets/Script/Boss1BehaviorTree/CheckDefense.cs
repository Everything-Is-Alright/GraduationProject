using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

// Condition：当玩家与boss的 X 轴绝对距离 <= maxDistance 时，每隔 rollInterval 秒做一次 1~10 的随机判定。
// 若随机到 1 则返回 Success，否则返回 Failure。
// 出范围时重置计时器。
public class CheckDefense : Conditional
{
    public SharedGameObject player;
    [Header("X轴最大距离（包含）")]
    public float maxDistance = 8f;
    [Header("随机判定间隔（秒）")]
    public float rollInterval = 1f;

    private float timer = 0f;
    private bool lastRollResult = false;

    public override TaskStatus OnUpdate()
    {
        // 优先使用黑板中的 SharedGameObject，否则回退到 Tag 查找
        GameObject target = (player != null && player.Value != null) ? player.Value : GameObject.FindWithTag("Player");
        if (target == null)
            return TaskStatus.Failure;

        float xDiff = Mathf.Abs(target.transform.position.x - transform.position.x);

        if (xDiff > maxDistance)
        {
            // 超出范围：重置计时并返回 Failure
            timer = 0f;
            lastRollResult = false;
            return TaskStatus.Failure;
        }

        // 在范围内，每隔 rollInterval 秒做一次随机判定
        timer += Time.deltaTime;
        if (timer >= rollInterval)
        {
            timer = 0f;
            int r = Random.Range(1, 11); 
            lastRollResult = (r == 1);
        }

        return lastRollResult ? TaskStatus.Success : TaskStatus.Failure;
    }
}