using UnityEngine;
using TMPro;
using System.Data;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.IO;

public class PackageManager : MonoBehaviour
{
    static PackageManager instance;

    public Package myPackageTemplate;
    public Slot slotPrefab;
    public GameObject slotGrid;
    public TextMeshProUGUI itemInfromation;

    public List<PackageItemData> packageItemData = new List<PackageItemData>();
    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        instance = this;

        LoadPackageData();
    }

    public static void CreateNewItem(PackageItemData item)
    {
        Slot newItem = Instantiate(instance.slotPrefab);
        newItem.transform.SetParent(instance.slotGrid.transform, false);
        RectTransform slotRect = newItem.GetComponent<RectTransform>();

        if(slotRect != null)
        {
            slotRect.anchoredPosition = Vector2.zero;
            slotRect.sizeDelta = Vector2.zero;
            slotRect.localScale = Vector3.one;
        }

        newItem.slotItem = item.itemTemplate;
        newItem.slotImage.sprite = item.itemTemplate.itemImage;
        newItem.slotImage.enabled = true;
        newItem.slotNum.text = item.itemCount.ToString();
    }

    private void OnEnable()
    {
        RefreshItem();
    }

    public static void RefreshItem()
    {
        if (instance == null || instance.slotGrid == null) return;

        // 清空现有Slot
        foreach (Transform child in instance.slotGrid.transform)
        {
            Destroy(child.gameObject);
        }

        // 遍历动态数据创建Slot
        for (int i = 0; i < instance.packageItemData.Count; i++)
        {
            CreateNewItem(instance.packageItemData[i]);
        }
    }

    public static void AddItemToPackage(Item itemTemplate)
    {
        if(instance == null || itemTemplate == null) return;

        PackageItemData existingItem = instance.packageItemData.Find(b => b.itemTemplate == itemTemplate);
        if(existingItem != null)
        {
            existingItem.itemCount++;
        }
        else
        {
            instance.packageItemData.Add(new PackageItemData{ itemTemplate = itemTemplate, itemCount = 1});
        }

        RefreshItem();
        instance.SavePackageData();
    }

    private string SavePath => Application.persistentDataPath + "/PackageSave.json";

    public void SavePackageData()
    {
        PackageSaveData saveData = new PackageSaveData();
        foreach(var packageItem in packageItemData)
        {
            saveData.itemNames.Add(packageItem.itemTemplate.itemName);
            saveData.itemCounts.Add(packageItem.itemCount);
        }

        string json = JsonUtility.ToJson(saveData,true);
        File.WriteAllText(SavePath, json);
        Debug.Log("游戏已保存！" + SavePath);
    }

    public void LoadPackageData()
    {
        packageItemData.Clear();

        if (!File.Exists(SavePath))
        {
            Debug.Log("当前没有存档文件，初始化空的背包！");
            return;
        }

        string json = File.ReadAllText(SavePath);
        PackageSaveData saveData = JsonUtility.FromJson<PackageSaveData>(json);

        for (int i = 0; i < saveData.itemNames.Count; i++)
        {
            Item itemTemplate = FindItemTemplateByName(saveData.itemNames[i]);

            if (itemTemplate != null)
            {
                packageItemData.Add(new PackageItemData { itemTemplate = itemTemplate, itemCount = saveData.itemCounts[i] });
            }

        }
        Debug.Log("读取成功，背包物品数量：" + packageItemData.Count);
    }

    private Item FindItemTemplateByName(string itemName)
    {
        return myPackageTemplate.itemList.Find(item => item.itemName == itemName);
    }
    
}
