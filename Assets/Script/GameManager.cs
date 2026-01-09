using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private PackageTable packageTable;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        UIManager.Instance.OpenPanel(UIConst.PackagePanel);
    }

    private void Update()
    {
        
    }

    public PackageTable GetPackageTable()
    {
        if(packageTable == null)
        {
            packageTable = Resources.Load<PackageTable>("Data/PackageTable");
        }
        return packageTable;
    }

    public List<PackageLocalItem> GetPackageLocalData()
    {
        return PackageLocalData.Instance.LoadPackage();
    }

    public PackageItem GetPackageItemById(int id)
    {
        List<PackageItem> packageDataList = GetPackageTable().DataList;
        foreach (PackageItem item in packageDataList)
        {
            if(item.id == id)
            {
                return item; 
            }
        }
        return null;
    }

    public PackageLocalItem GetPackageLocalItemByUID(string uid)
    {
        List<PackageLocalItem> packageDataList = GetPackageLocalData();
        foreach (PackageLocalItem item in packageDataList)
        {
            if(item.uid == uid)
            {
                return item; 
            }
        }
        return null;
    }

    public List<PackageLocalItem> GetSortPackageLocalData()
    {
        List<PackageLocalItem> localItems = PackageLocalData.Instance.LoadPackage();
        localItems.Sort(new PackageItemComparer());
        return localItems;
    }

    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }
}


public class PackageItemComparer : IComparer<PackageLocalItem>
{
    public int Compare(PackageLocalItem a, PackageLocalItem b)
    {
        PackageItem x = GameManager.Instance.GetPackageItemById(a.id);
        PackageItem y = GameManager.Instance.GetPackageItemById(b.id);

        int starComparison = y.star.CompareTo(x.star);

        if(starComparison == 0 )
        {
            int idComparison = y.id.CompareTo(x.id);
            if(idComparison == 0 )
            {
                return b.level.CompareTo(a.level);
            }
            return idComparison;
        }
        return starComparison;
    }
}