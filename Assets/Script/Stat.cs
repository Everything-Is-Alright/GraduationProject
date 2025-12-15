using System;
using UnityEngine;

[Serializable]
//这个类专门用于储存那些会影响数值计算的buff，比如药水，装备等,并完成计算
public class Stat
{
    [SerializeField] private float baseValue;

    public float GetValue()
    {
        return baseValue;
    }
}
