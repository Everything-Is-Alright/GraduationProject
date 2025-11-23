using System;
using UnityEngine;

public class EntityAnimationTrigger : MonoBehaviour
{
    private IEntity entity;

    private void Awake()
    {
        entity = GetComponentInParent<IEntity>();
    }

    public void CurrentStateTrigger()
    {
        entity.CallAnimationTrigger();
    }
}