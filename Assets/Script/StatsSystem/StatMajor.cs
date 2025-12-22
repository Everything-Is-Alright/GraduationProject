using System;
using UnityEngine;

//该脚本设置玩家基础属性
[Serializable]
public class StatMajor
{    
    //力量，会提高攻击力
    public Stat strength;
    //敏捷，会提高暴击率，+0.3/级
    public Stat agility;
    //智力
    public Stat intelligence;
    //生命
    public Stat vitality;
}
