using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PackageManager : MonoBehaviour
{
    // 场景单例
    public static PackageManager Instance { get; private set; }

    [Header("模板数据（用于从 id 恢复模板）")]
    public Package slotItem; // 包含 itemList 的 ScriptableObject（保留，用于加载时恢复模板）

    private PackageModel model;
    private IPackageRepository repository;

    // 外部订阅：数据变化时通知 View
    public event Action OnPackageChanged;
    public event Action<string, int> OnItemAdded;
    public event Action<string, int> OnItemRemoved;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        model = new PackageModel();
        repository = new JsonPackageRepository();

        // 事件转发
        model.OnChanged += () => OnPackageChanged?.Invoke();
        model.OnItemAdded += (id, cnt) => OnItemAdded?.Invoke(id, cnt);
        model.OnItemRemoved += (id, cnt) => OnItemRemoved?.Invoke(id, cnt);

        Load();
    }

    #region Public API (实例方法)
    public void AddItem(Item template, int count = 1)
    {
        model.AddItem(template, count);
        Save();
    }

    public void RemoveItem(string itemId, int count = 1)
    {
        model.RemoveItem(itemId, count);
        Save();
    }

    public int GetItemCount(string itemId) => model.GetItemCount(itemId);

    public bool HasItem(string itemId) => model.HasItem(itemId);

    public IReadOnlyDictionary<string, PackageItemData> GetAllItems() => model.Items;
    #endregion

    #region 静态兼容 API（便于逐步替换旧调用）
    public static void AddItemToPackage(Item itemTemplate)
    {
        if (Instance == null || itemTemplate == null) return;
        Instance.AddItem(itemTemplate, 1);
        Debug.Log("已添加物品：" + itemTemplate.itemName);
    }

    public static void RemoveItemFromPackage(string itemId, int count = 1)
    {
        if (Instance == null) return;
        Instance.RemoveItem(itemId, count);
    }

    public static int GetItemCountStatic(string itemId)
    {
        if (Instance == null) return 0;
        return Instance.GetItemCount(itemId);
    }

    // 兼容旧的 HasItem 调用（若存在）
    public static bool HasItemStatic(string itemId)
    {
        if (Instance == null) return false;
        return Instance.HasItem(itemId);
    }

    #endregion

    #region Save / Load
    private void Save()
    {
        try
        {
            repository.Save(model.ToSerializableDictionary());
        }
        catch (Exception ex)
        {
            Debug.LogWarning("保存背包失败: " + ex);
        }
    }

    private void Load()
    {
        var raw = repository.LoadRaw();

        var dict = new Dictionary<string, PackageItemData>();

        // 将保存的 id/count 映射回模板（slotItem.itemList）
        if (raw != null && raw.itemIds != null)
        {
            for (int i = 0; i < raw.itemIds.Count; i++)
            {
                string id = raw.itemIds[i];
                int cnt = i < raw.itemCounts.Count ? raw.itemCounts[i] : 0;
                if (string.IsNullOrEmpty(id) || cnt <= 0) continue;

                Item template = FindTemplateById(id);
                if (template != null)
                {
                    dict[id] = new PackageItemData { itemTemplate = template, itemCount = cnt };
                }
                else
                {
                    // 未在模板表中找到，跳过（或可实现一个“本地表”回填机制）
                    Debug.LogWarning($"加载背包时未找到模板 id={id}");
                }
            }
        }

        model.SetAll(dict);
    }

    private Item FindTemplateById(string id)
    {
        if (slotItem == null || slotItem.itemList == null) return null;
        return slotItem.itemList.FirstOrDefault(it => it != null && it.itemId == id);
    }
    #endregion

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }
}