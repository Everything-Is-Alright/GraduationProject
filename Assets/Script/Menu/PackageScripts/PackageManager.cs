using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class PackageManager : MonoBehaviour
{
    private static PackageManager instance;
    public static PackageManager Instance => instance;

    [Header("Prefab & References")]
    public Package slotItem;
    public Slot slotPrefab;

    [Header("Panels")]
    public GameObject weaponPanel;
    public GameObject armorPanel;
    public GameObject accessoriesPanel;
    public GameObject propPanel;
    public GameObject taskItemPanel;

    [Header("Current State")]
    public ItemType currentShowType = ItemType.Weapon;

    private Dictionary<string, Item> itemTemplateDict;

    private Dictionary<string, PackageItemData> itemDataDict = new Dictionary<string, PackageItemData>();

    private Dictionary<ItemType, Dictionary<string, Slot>> pageSlotMaps = new Dictionary<ItemType, Dictionary<string, Slot>>();

    private Dictionary<ItemType, GameObject> panelDict = new Dictionary<ItemType, GameObject>();

    private List<Slot> slotPool = new List<Slot>();
    private Transform poolContainer;

    private HashSet<string> currentTypeItemIds = new HashSet<string>();

    public IReadOnlyDictionary<string, PackageItemData> ItemDataDict => itemDataDict;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        InitializePoolContainer();
        InitializePanelDict();
        InitItemTemplateDict();
        LoadPackageData();

        HideAllPages();
        weaponPanel.SetActive(true);
    }

    private void InitializePoolContainer()
    {
        GameObject poolObj = new GameObject("SlotPool");
        poolObj.transform.SetParent(transform);
        poolContainer = poolObj.transform;
    }

    private void InitializePanelDict()
    {
        panelDict[ItemType.Weapon] = weaponPanel;
        panelDict[ItemType.Armor] = armorPanel;
        panelDict[ItemType.Accessories] = accessoriesPanel;
        panelDict[ItemType.Prop] = propPanel;
        panelDict[ItemType.TaskItem] = taskItemPanel;

        foreach (var type in System.Enum.GetValues(typeof(ItemType)).Cast<ItemType>())
        {
            if (!pageSlotMaps.ContainsKey(type))
            {
                pageSlotMaps[type] = new Dictionary<string, Slot>();
            }
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

        foreach (var item in slotItem.itemList)
        {
            if (!string.IsNullOrEmpty(item.itemId) && !itemTemplateDict.ContainsKey(item.itemId))
            {
                itemTemplateDict.Add(item.itemId, item);
            }
        }
    }

    private void OnEnable()
    {
        RefreshCurrentPage();
    }

    private GameObject GetPagePanelByType(ItemType type)
    {
        return panelDict.TryGetValue(type, out var panel) ? panel : null;
    }

    private Slot GetSlotFromPool()
    {
        for (int i = slotPool.Count - 1; i >= 0; i--)
        {
            if (slotPool[i] != null)
            {
                Slot slot = slotPool[i];
                slotPool.RemoveAt(i);
                return slot;
            }
        }

        Slot newSlot = Instantiate(slotPrefab, poolContainer);
        return newSlot;
    }

    private void ReturnSlotToPool(Slot slot)
    {
        if (slot == null) return;

        slot.transform.SetParent(poolContainer, false);
        slot.gameObject.SetActive(false);
        slot.slotItem = null;
        slotPool.Add(slot);
    }

    public void RefreshCurrentPage()
    {
        if (instance == null) return;

        GameObject currentPage = GetPagePanelByType(currentShowType);
        if (currentPage == null) return;

        Dictionary<string, Slot> slotMap = pageSlotMaps[currentShowType];

        currentTypeItemIds.Clear();

        foreach (var kvp in itemDataDict)
        {
            PackageItemData itemData = kvp.Value;
            if (itemData.itemTemplate == null) continue;

            if (itemData.itemTemplate.itemType == currentShowType)
            {
                currentTypeItemIds.Add(kvp.Key);

                if (slotMap.TryGetValue(kvp.Key, out Slot existingSlot))
                {
                    UpdateSlot(existingSlot, itemData);
                }
                else
                {
                    Slot newSlot = CreateSlot(itemData, currentPage.transform);
                    slotMap[kvp.Key] = newSlot;
                }
            }
        }

        List<string> slotsToRemove = null;
        foreach (var kvp in slotMap)
        {
            if (!currentTypeItemIds.Contains(kvp.Key))
            {
                if (slotsToRemove == null)
                    slotsToRemove = new List<string>();
                slotsToRemove.Add(kvp.Key);
            }
        }

        if (slotsToRemove != null)
        {
            foreach (string itemId in slotsToRemove)
            {
                if (slotMap.TryGetValue(itemId, out Slot slot))
                {
                    ReturnSlotToPool(slot);
                    slotMap.Remove(itemId);
                }
            }
        }
    }

    private Slot CreateSlot(PackageItemData itemData, Transform parent)
    {
        Slot slot = GetSlotFromPool();
        slot.transform.SetParent(parent, false);
        slot.gameObject.SetActive(true);

        RectTransform slotRect = slot.GetComponent<RectTransform>();
        if (slotRect != null)
        {
            slotRect.anchoredPosition = Vector2.zero;
            slotRect.sizeDelta = Vector2.zero;
            slotRect.localScale = Vector3.one;
        }

        UpdateSlot(slot, itemData);
        return slot;
    }

    private void UpdateSlot(Slot slot, PackageItemData itemData)
    {
        slot.slotItem = itemData.itemTemplate;
        if (slot.slotImage != null)
        {
            slot.slotImage.sprite = itemData.itemTemplate.itemImage;
            slot.slotImage.enabled = true;
        }
        if (slot.slotNum != null)
        {
            slot.slotNum.text = itemData.itemCount.ToString();
        }
    }

    public static void AddItemToPackage(Item itemTemplate)
    {
        if (instance == null || itemTemplate == null) return;
        if (string.IsNullOrEmpty(itemTemplate.itemId))
        {
            Debug.LogWarning("物品缺少itemId，无法添加！");
            return;
        }

        string itemId = itemTemplate.itemId;

        if (instance.itemDataDict.TryGetValue(itemId, out PackageItemData existingItem))
        {
            existingItem.itemCount++;
        }
        else
        {
            PackageItemData newItem = new PackageItemData
            {
                itemTemplate = itemTemplate,
                itemCount = 1
            };
            instance.itemDataDict.Add(itemId, newItem);
        }

        if (itemTemplate.itemType == instance.currentShowType)
        {
            instance.RefreshCurrentPage();
        }

        instance.SavePackageData();
    }

    public static void RemoveItemFromPackage(string itemId, int count = 1)
    {
        if (instance == null || string.IsNullOrEmpty(itemId)) return;

        if (instance.itemDataDict.TryGetValue(itemId, out PackageItemData itemData))
        {
            itemData.itemCount -= count;

            if (itemData.itemCount <= 0)
            {
                instance.itemDataDict.Remove(itemId);

                if (instance.pageSlotMaps[instance.currentShowType].TryGetValue(itemId, out Slot slot))
                {
                    instance.ReturnSlotToPool(slot);
                    instance.pageSlotMaps[instance.currentShowType].Remove(itemId);
                }
            }
            else
            {
                if (instance.pageSlotMaps[instance.currentShowType].TryGetValue(itemId, out Slot slot))
                {
                    instance.UpdateSlot(slot, itemData);
                }
            }

            instance.SavePackageData();
        }
    }

    public static PackageItemData GetItemData(string itemId)
    {
        if (instance == null || string.IsNullOrEmpty(itemId)) return null;
        return instance.itemDataDict.TryGetValue(itemId, out var data) ? data : null;
    }

    public static int GetItemCount(string itemId)
    {
        var data = GetItemData(itemId);
        return data?.itemCount ?? 0;
    }

    private string SavePath => Application.persistentDataPath + "/PackageSave.json";

    public void SavePackageData()
    {
        PackageSaveData saveData = new PackageSaveData();
        foreach (var kvp in itemDataDict)
        {
            PackageItemData packageItem = kvp.Value;
            if (packageItem.itemTemplate == null) continue;

            saveData.itemIds.Add(packageItem.itemTemplate.itemId);
            saveData.itemNames.Add(packageItem.itemTemplate.itemName);
            saveData.itemCounts.Add(packageItem.itemCount);
        }

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(SavePath, json);
        Debug.Log("游戏已保存！" + SavePath);
    }

    public void LoadPackageData()
    {
        itemDataDict.Clear();

        if (!File.Exists(SavePath))
        {
            Debug.Log("当前没有存档文件，初始化空的背包数据");
            return;
        }

        string json = File.ReadAllText(SavePath);
        PackageSaveData saveData = JsonUtility.FromJson<PackageSaveData>(json);

        if (saveData.itemIds == null || saveData.itemIds.Count == 0)
        {
            Debug.Log("存档数据为空");
            return;
        }

        int dataCount = Mathf.Min(saveData.itemIds.Count, saveData.itemCounts.Count);

        for (int i = 0; i < dataCount; i++)
        {
            string itemId = saveData.itemIds[i];
            Item itemTemplate = FindItemTemplate(itemId);

            if (itemTemplate != null)
            {
                itemDataDict[itemId] = new PackageItemData
                {
                    itemTemplate = itemTemplate,
                    itemCount = Mathf.Max(1, saveData.itemCounts[i])
                };
            }
            else
            {
                Debug.LogWarning($"未找到物品模板: {itemId}");
            }
        }

        Debug.Log($"读取成功，背包物品数量: {itemDataDict.Count}");
        RefreshCurrentPage();
    }

    private Item FindItemTemplate(string itemId)
    {
        return itemTemplateDict.TryGetValue(itemId, out Item item) ? item : null;
    }

    public void OnClickWeaponToggle() => SwitchPage(ItemType.Weapon);
    public void OnClickArmorToggle() => SwitchPage(ItemType.Armor);
    public void OnClickAccessoriesToggle() => SwitchPage(ItemType.Accessories);
    public void OnClickPropToggle() => SwitchPage(ItemType.Prop);
    public void OnClickTaskItemToggle() => SwitchPage(ItemType.TaskItem);

    private void SwitchPage(ItemType type)
    {
        if (currentShowType == type) return;

        currentShowType = type;
        HideAllPages();

        GameObject panel = GetPagePanelByType(type);
        if (panel != null)
        {
            panel.SetActive(true);
            RefreshCurrentPage();
        }
    }

    private void HideAllPages()
    {
        weaponPanel.SetActive(false);
        armorPanel.SetActive(false);
        accessoriesPanel.SetActive(false);
        propPanel.SetActive(false);
        taskItemPanel.SetActive(false);
    }

    public IReadOnlyList<PackageItemData> GetItemsByType(ItemType type)
    {
        var result = new List<PackageItemData>();
        foreach (var kvp in itemDataDict)
        {
            if (kvp.Value.itemTemplate != null && kvp.Value.itemTemplate.itemType == type)
            {
                result.Add(kvp.Value);
            }
        }
        return result;
    }

    public bool HasItem(string itemId)
    {
        return itemDataDict.ContainsKey(itemId);
    }
}
