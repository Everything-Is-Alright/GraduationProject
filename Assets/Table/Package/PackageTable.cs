using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "XiaoQi/PackageTable", fileName = "PackageTable")]
public class PackageTable : ScriptableObject
{
    public List<PackageItem> DataList = new List<PackageItem>();
}