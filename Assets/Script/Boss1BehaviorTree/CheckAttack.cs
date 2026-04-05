using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class CheckAttack : Conditional
{
    public SharedGameObject player;
    [Header("X轴攻击范围")]
    public float attackRangeX = 15f;    

    // 指定本条件检查的攻击类型（与 AttackCooldown 中枚举对应）
    public AttackType attackType = AttackType.Attack1;

    public override TaskStatus OnUpdate()
    {
        // 优先使用黑板中的 SharedGameObject（若设置了），否则回退到通过 Tag 查找
        GameObject target = (player != null && player.Value != null) ? player.Value : GameObject.FindWithTag("Player");
        if (target == null)
        {
            // 找不到玩家则判为 Failure（也可根据设计改为 Success）
            return TaskStatus.Failure;
        }

        // 若挂载了 AttackCooldown，则在条件中检查冷却状态
        var cooldown = gameObject.GetComponent<AttackCooldown>();
        if (cooldown != null && !cooldown.CanAttack(attackType))
        {
            // 在冷却中，不允许攻击
            return TaskStatus.Failure;
        }

        float xDiff = Mathf.Abs(target.transform.position.x - transform.position.x);
        return xDiff <= attackRangeX ? TaskStatus.Success : TaskStatus.Failure;
    }
}