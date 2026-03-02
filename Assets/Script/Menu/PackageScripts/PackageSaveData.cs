using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PackageSaveData
{
    public List<string> itemIds = new List<string>();
    public List<string> itemNames = new List<string>();
    public List<int> itemCounts = new List<int>();
}
