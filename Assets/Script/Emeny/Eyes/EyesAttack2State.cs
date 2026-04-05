﻿using UnityEngine;

public class EyesAttack2State : EntityState<Eyes>
{
    public EyesAttack2State(Eyes eyes, StateMachine<Eyes> stateMachine, string animBoolName) : base(eyes, stateMachine, animBoolName)
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
        // 每帧强制静止
        entity.SetVelocity(0, 0);
        entity.rb.linearVelocity = Vector2.zero;

        if (entity.player == null || Vector2.Distance(entity.transform.position, entity.player.position) > entity.attackDistance)
        {
            entity.ClearPlayerReference();
            stateMachine.ChangeState(entity.FlyState);
            return;
        }

        // 动画结束 → 返回飞行
        if (triggerCalled)
        {
            entity.ClearPlayerReference();
            stateMachine.ChangeState(entity.FlyState);
        }
    }
}