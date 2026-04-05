﻿using UnityEngine;

public class EyesAttack1State : EntityState<Eyes>
{
    public EyesAttack1State(Eyes eyes, StateMachine<Eyes> stateMachine, string animBoolName) : base(eyes, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        // 进入攻击：立刻静止
        entity.SetVelocity(0, 0);
        entity.rb.linearVelocity = Vector2.zero;
    }

    public override void Update()
    {
        base.Update();
        // 每帧强制静止（绝对不动）
        entity.SetVelocity(0, 0);
        entity.rb.linearVelocity = Vector2.zero;
        if (entity.player == null || Vector2.Distance(entity.transform.position, entity.player.position) > entity.attackDistance)
        {
            entity.ClearPlayerReference();
            stateMachine.ChangeState(entity.FlyState);
            return;
        }

        // 动画触发 → 连招
        if (triggerCalled)
        {
            stateMachine.ChangeState(entity.Attack2State);
        }
    }
}