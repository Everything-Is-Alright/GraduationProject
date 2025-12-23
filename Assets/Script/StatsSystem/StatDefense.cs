using System;
using UnityEngine;

//该脚本负责计算玩家防御数值
[Serializable]
public class StatDefense
{
    //物理抗性
    public Stat armor;
    public Stat evasion;

    //元素抗性
    public Stat magicRes;
}
