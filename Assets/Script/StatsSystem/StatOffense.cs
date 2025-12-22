using System;
using UnityEngine;

//该脚本设置玩家进攻行为数值
[Serializable]
public class StatOffense
{
    //物理伤害
    public Stat damage;
    public Stat critPower;
    //基础暴击概率
    public Stat critChance;

    //元素伤害
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightningDamage;
}
