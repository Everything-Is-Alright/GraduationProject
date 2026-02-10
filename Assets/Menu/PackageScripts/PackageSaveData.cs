using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PackageSaveData
{
    //物品名称列表（匹配Item SO的itemName）
    public List<string> itemNames = new List<string>();
    //对应物品的数量列表
    public List<int> itemCounts = new List<int>();
}
