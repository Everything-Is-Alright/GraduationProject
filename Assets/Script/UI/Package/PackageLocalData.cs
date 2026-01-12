using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Search;

public class PackageLocalData
{
    private static PackageLocalData instance;

    public static PackageLocalData Instance
    {
        get 
        { 
            if(instance == null)
            {
                instance = new PackageLocalData();
                instance.LoadPackage();
            }
            return instance;
        }
    }

    public List<PackageLocalItem> items;

    public void SavePackage()
    {
        string inventoryJson = JsonUtility.ToJson(this);
        PlayerPrefs.SetString("PackageLocalData", inventoryJson);
        PlayerPrefs.Save();
    }

    public List<PackageLocalItem> LoadPackage()
    {
        if(items != null )
        {
            return items;
        }
        if(PlayerPrefs.HasKey("PackageLocalData"))
        {
            string inventoryJson = PlayerPrefs.GetString("PackageLocalData");
            PackageLocalData packageLocalData = JsonUtility.FromJson<PackageLocalData>(inventoryJson);
            items = packageLocalData.items;
            return items;
        }
        else
        {
            items = new List<PackageLocalItem>();
            return items;
        }
    }
}
