using UnityEngine;
using TMPro;
using System.Data;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.IO;

public class PackageManager : MonoBehaviour
{
    private static PackageManager instance;

    public static PackageManager Instance
    {
        get
        {
            return instance;
        }
    }

    public Package slotItem;
    public Slot slotPrefab;
    public GameObject slotGrid;

    public GameObject weaponPanel;
    public GameObject armorPanel;
    public GameObject accessoriesPanel;
    public GameObject propPanel;
    public GameObject taskItemPanel;

    private Dictionary<string, Item> itemTemplateDict;


    public ItemType currentShowType = ItemType.Weapon;

    public List<PackageItemData> packageItemData = new List<PackageItemData>();
    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        DontDestroyOnLoad(gameObject);
        InitItemTemplateDict();
        LoadPackageData();
        
        HideAllPages();
        weaponPanel.SetActive(true);
    }

    public static void CreateNewItem(PackageItemData item, GameObject parentPanel)
    {
        Slot newItem = Instantiate(instance.slotPrefab);
        newItem.transform.SetParent(parentPanel.transform, false);

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

    private GameObject GetPagePanelByType(ItemType type)
    {
        switch (type)
        {
            case ItemType.Weapon:
                return weaponPanel;
            case ItemType.Armor:
                return armorPanel;
            case ItemType.Accessories:
                return accessoriesPanel;
            case ItemType.Prop:
                return propPanel;
            case ItemType.TaskItem:
                return taskItemPanel;
            default:
                return null;
        }
    }

    private void InitItemTemplateDict()
    {
        itemTemplateDict = new Dictionary<string, Item>();

        if (slotItem == null || slotItem.itemList == null)
        {
            Debug.LogError("SlotItem或物品列表为空，字典初始化失败！");
            return;
        }

        if (slotItem != null && slotItem.itemList != null)
        {
            foreach (var item in slotItem.itemList)
            {
                if (!itemTemplateDict.ContainsKey(item.itemId))
                {
                    itemTemplateDict.Add(item.itemId, item);
                }
            }
        }
    }

    public static void RefreshItem()
    {
        if (instance == null) return;

        GameObject currentPage = instance.GetPagePanelByType(instance.currentShowType);
        List<PackageItemData> fitItems = instance.packageItemData.FindAll(item =>item.itemTemplate.itemType == instance.currentShowType);

        Dictionary<Item, Slot> existingSlotMap = new Dictionary<Item, Slot>();
        List<Slot> allExistingSlots = new List<Slot>();

        foreach (Transform child in currentPage.transform)
        {
            Slot slot = child.GetComponent<Slot>();
            if (slot != null && slot.slotItem != null)
            {
                if (!existingSlotMap.ContainsKey(slot.slotItem))
                {
                    existingSlotMap.Add(slot.slotItem, slot);
                }
                allExistingSlots.Add(slot);
            }
        }

        foreach (var itemData in fitItems)
        {
            if (existingSlotMap.TryGetValue(itemData.itemTemplate, out Slot existingSlot))
            {
                existingSlot.slotNum.text = itemData.itemCount.ToString();
                existingSlotMap.Remove(itemData.itemTemplate);
            }
            else
            {
                CreateNewItem(itemData, currentPage);
            }
        }

        foreach (var leftoverSlot in existingSlotMap.Values)
        {
            if (leftoverSlot != null)
            {
                Destroy(leftoverSlot.gameObject);
            }
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
        foreach (var packageItem in packageItemData)
        {
            if (packageItem.itemTemplate == null) continue;
            saveData.itemIds.Add(packageItem.itemTemplate.itemId);
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

        int dataCount = Mathf.Min(saveData.itemIds != null ? saveData.itemIds.Count : 0,saveData.itemCounts.Count);

        if (saveData.itemIds != null && saveData.itemIds.Count > 0)
        {
            for (int i = 0; i < dataCount; i++)
            {
                string itemId = saveData.itemIds[i];
                Item itemTemplate = FindItemTemplate(itemId);

                if (itemTemplate != null)
                {
                    packageItemData.Add(new PackageItemData
                    {
                        itemTemplate = itemTemplate,
                        itemCount = Mathf.Max(1, saveData.itemCounts[i])
                    });
                }
                else
                {
                    Debug.LogWarning("未找到物品，跳过加载！");
                }
            }
        }
        Debug.Log("读取成功，背包物品数量：" + packageItemData.Count);

        RefreshItem();
    }

    private Item FindItemTemplate(string itemKey)
    {
        if (itemTemplateDict.TryGetValue(itemKey, out Item itemById))
        {
            return itemById;
        }

        Debug.LogWarning($"未找到物品");
        return null;
    }

    public void OnClickWeaponToggle()
    {
        currentShowType = ItemType.Weapon;
        HideAllPages();
        weaponPanel.SetActive(true);
        RefreshItem();
    }

    public void OnClickArmorToggle()
    {
        currentShowType = ItemType.Armor;
        HideAllPages();
        armorPanel.SetActive(true);
        RefreshItem();
    }

    public void OnClickAccessoriesToggle()
    {
        currentShowType = ItemType.Accessories;
        HideAllPages();
        accessoriesPanel.SetActive(true);
        RefreshItem();
    }

    public void OnClickPropToggle()
    {
        currentShowType = ItemType.Prop;
        HideAllPages();
        propPanel.SetActive(true);
        RefreshItem();
    }

    public void OnClickTaskItemToggle()
    {
        currentShowType = ItemType.TaskItem;
        HideAllPages();
        taskItemPanel.SetActive(true);
        RefreshItem();
    }

    private void HideAllPages()
    {
        weaponPanel.SetActive(false);
        armorPanel.SetActive(false);
        accessoriesPanel.SetActive(false);
        propPanel.SetActive(false);
        taskItemPanel.SetActive(false);
    }

}
