using System;
using UnityEngine;

public class EntityAnimationTrigger : MonoBehaviour
{
    private IEntity entity;
    private EntityCombat entityCombat;

    private void Awake()
    {
        entity = GetComponentInParent<IEntity>();
        entityCombat = GetComponentInParent<EntityCombat>();
    }

    public void CurrentStateTrigger()
    {
        entity.CallAnimationTrigger();
    }

    public void AttackTrigger()
    {
        entityCombat.PerformAttack();
    }
}