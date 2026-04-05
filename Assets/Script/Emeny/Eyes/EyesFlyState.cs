using UnityEngine;

public class EyesFlyState : EntityState<Eyes>
{
    // 冷却：防止反复切换攻击状态
    private float attackCooldown = 0.5f;
    private float cooldownTimer;

    public EyesFlyState(Eyes eyes, StateMachine<Eyes> stateMachine, string animBoolName) : base(eyes, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        cooldownTimer = 0;
    }

    public override void Update()
    {
        base.Update();
        cooldownTimer -= Time.deltaTime;

        // 撞墙反转
        if (entity.isWallDetected)
        {
            entity.Flip();
            entity.SetVelocity(0, entity.rb.linearVelocityY);
            return;
        }

        // 搜索玩家
        if (entity.player == null)
        {
            entity.GetPlayerRefence();
        }

        // 玩家在范围内 + 冷却结束 → 进入攻击
        if (entity.player != null && cooldownTimer <= 0)
        {
            float distance = Vector2.Distance(entity.transform.position, entity.player.position);
            if (distance <= entity.attackDistance)
            {
                cooldownTimer = attackCooldown;
                stateMachine.ChangeState(entity.Attack1State);
                return;
            }
        }

        // 正常飞行移动
        entity.SetVelocity(entity.battleMoveSpeed * entity.entityFacing, entity.rb.linearVelocityY);
    }
}