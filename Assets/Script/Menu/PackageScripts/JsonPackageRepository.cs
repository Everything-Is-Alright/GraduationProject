using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonPackageRepository : IPackageRepository
{
    private readonly string path = Application.persistentDataPath + "/PackageSave.json";

    public void Save(Dictionary<string, PackageItemData> data)
    {
        var save = new PackageSaveData();
        foreach (var d in data.Values)
        {
            if (d?.itemTemplate == null) continue;
            save.itemIds.Add(d.itemTemplate.itemId);
            save.itemCounts.Add(d.itemCount);
        }

        try
        {
            File.WriteAllText(path, JsonUtility.ToJson(save));
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning("Package ±£¥Ê ß∞‹: " + ex);
        }
    }

    public PackageSaveData LoadRaw()
    {
        if (!File.Exists(path)) return new PackageSaveData();
        try
        {
            string txt = File.ReadAllText(path);
            return JsonUtility.FromJson<PackageSaveData>(txt) ?? new PackageSaveData();
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning("Package ∂¡»° ß∞‹: " + ex);
            return new PackageSaveData();
        }
    }
}