using System;
using System.Collections.Generic;

public class PackageModel
{
    // 通用事件：数据发生任意变化
    public event Action OnChanged;

    // 更细粒度事件（可选订阅）
    public event Action<string, int> OnItemAdded;
    public event Action<string, int> OnItemRemoved;

    private readonly Dictionary<string, PackageItemData> items = new Dictionary<string, PackageItemData>();

    public IReadOnlyDictionary<string, PackageItemData> Items => items;

    public int GetItemCount(string itemId) => string.IsNullOrEmpty(itemId) ? 0 : (items.TryGetValue(itemId, out var d) ? d.itemCount : 0);

    public bool HasItem(string itemId) => GetItemCount(itemId) > 0;

    public void AddItem(Item template, int count = 1)
    {
        if (template == null || string.IsNullOrEmpty(template.itemId) || count <= 0) return;

        if (items.TryGetValue(template.itemId, out var d))
            d.itemCount += count;
        else
            items[template.itemId] = new PackageItemData { itemTemplate = template, itemCount = count };

        OnItemAdded?.Invoke(template.itemId, count);
        OnChanged?.Invoke();
    }

    public void RemoveItem(string itemId, int count = 1)
    {
        if (string.IsNullOrEmpty(itemId) || count <= 0) return;
        if (!items.TryGetValue(itemId, out var d)) return;

        int removed = Math.Min(count, d.itemCount);
        d.itemCount -= removed;
        if (d.itemCount <= 0) items.Remove(itemId);

        OnItemRemoved?.Invoke(itemId, removed);
        OnChanged?.Invoke();
    }

    public void SetAll(Dictionary<string, PackageItemData> newData)
    {
        items.Clear();
        if (newData != null)
        {
            foreach (var kv in newData) items[kv.Key] = kv.Value;
        }
        OnChanged?.Invoke();
    }

    public Dictionary<string, PackageItemData> ToSerializableDictionary()
    {
        // 返回数据快照（Repository 使用）
        return new Dictionary<string, PackageItemData>(items);
    }
}