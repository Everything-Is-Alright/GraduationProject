using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// View：订阅 PackageManager 的事件并刷新面板（使用现有 Slot prefab 等）
// 设计要点：不依赖 PackageManager 挂在同一物体，处理 Manager 延迟创建，防止重复订阅
public class PackageUIView : MonoBehaviour
{
    public Slot slotPrefab;
    public GameObject weaponPanel;
    public GameObject armorPanel;
    public GameObject accessoriesPanel;
    public GameObject propPanel;
    public GameObject taskItemPanel;

    public ItemType currentShowType = ItemType.Weapon;

    private PackageManager controller;
    private bool subscribed = false;
    private Coroutine waitCoroutine;

    private void Awake()
    {
        // 优先使用单例（推荐将 PackageManager 放在 Managers 根对象并设置为 DontDestroyOnLoad）
        controller = PackageManager.Instance;
    }

    private void OnEnable()
    {
        // 若尚未拿到 controller，则尝试查找或等待
        if (controller == null)
        {
            controller = PackageManager.Instance ?? FindObjectOfType<PackageManager>();
        }

        if (controller != null)
        {
            SubscribeAndRefresh();
        }
        else
        {
            // 等待 PackageManager 实例化（防止初始化顺序问题）
            waitCoroutine = StartCoroutine(WaitForManagerAndSubscribe());
        }
    }

    private void OnDisable()
    {
        Unsubscribe();
        if (waitCoroutine != null)
        {
            StopCoroutine(waitCoroutine);
            waitCoroutine = null;
        }
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

    private IEnumerator WaitForManagerAndSubscribe()
    {
        int tries = 0;
        while ((controller == null) && tries < 60) // 最多等待 60 帧
        {
            controller = PackageManager.Instance ?? FindObjectOfType<PackageManager>();
            if (controller != null) break;
            tries++;
            yield return null;
        }

        if (controller != null)
        {
            SubscribeAndRefresh();
        }
        else
        {
            Debug.LogWarning("[PackageUIView] 未找到 PackageManager（等待超时），请确保场景中存在该组件。");
        }

        waitCoroutine = null;
    }

    private void SubscribeAndRefresh()
    {
        if (controller == null) return;
        if (subscribed) return;

        controller.OnPackageChanged += RefreshView;
        controller.OnItemAdded += OnItemAdded;
        controller.OnItemRemoved += OnItemRemoved;
        subscribed = true;

        // 立即刷新以同步 UI
        RefreshView();

        // 打印背包内容 —— 仅在打开背包（首次订阅/刷新）时执行
        PrintAllItems();
    }

    private void Unsubscribe()
    {
        if (controller == null || !subscribed) return;

        controller.OnPackageChanged -= RefreshView;
        controller.OnItemAdded -= OnItemAdded;
        controller.OnItemRemoved -= OnItemRemoved;
        subscribed = false;
    }

    private void OnItemAdded(string itemId, int count)
    {
        // 若新增物品属于当前页则刷新；否则不做任何事以减少重绘
        if (IsItemOfCurrentPage(itemId))
            RefreshView();
    }

    private void OnItemRemoved(string itemId, int count)
    {
        // 若移除物品影响当前页则刷新
        if (IsItemOfCurrentPage(itemId))
            RefreshView();
    }

    private bool IsItemOfCurrentPage(string itemId)
    {
        if (controller == null) return false;
        var items = controller.GetAllItems();
        if (items == null) return false;
        if (!items.TryGetValue(itemId, out var data)) return false;
        return data?.itemTemplate != null && data.itemTemplate.itemType == currentShowType;
    }

    public void RefreshView()
    {
        if (controller == null) return;

        var dict = new Dictionary<ItemType, GameObject>
        {
            { ItemType.Weapon, weaponPanel },
            { ItemType.Armor, armorPanel },
            { ItemType.Accessories, accessoriesPanel },
            { ItemType.Prop, propPanel },
            { ItemType.TaskItem, taskItemPanel }
        };

        if (!dict.TryGetValue(currentShowType, out var parent)) return;
        if (parent == null) return;

        // 清空
        foreach (Transform child in parent.transform) Destroy(child.gameObject);

        // 填充
        foreach (var kv in controller.GetAllItems().Values)
        {
            if (kv.itemTemplate == null || kv.itemTemplate.itemType != currentShowType) continue;
            if (slotPrefab == null) continue;

            Slot slot = Instantiate(slotPrefab, parent.transform);
            slot.gameObject.SetActive(true);
            slot.slotItem = kv.itemTemplate;
            slot.slotImage.sprite = kv.itemTemplate.itemImage;
            slot.slotImage.enabled = true;
            slot.slotNum.text = kv.itemCount > 1 ? kv.itemCount.ToString() : "";
        }
    }

    // 打印背包中所有物品（在打开背包时调用）
    private void PrintAllItems()
    {
        if (controller == null)
        {
            Debug.Log("[PackageUIView] 无法打印背包：未找到 PackageManager");
            return;
        }

        var all = controller.GetAllItems();
        if (all == null || all.Count == 0)
        {
            Debug.Log("[Package] 背包为空");
            return;
        }

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.AppendLine("[Package] 当前背包物品：");
        foreach (var kv in all)
        {
            var id = kv.Key;
            var data = kv.Value;
            if (data?.itemTemplate == null) continue;
            sb.AppendLine($"- {data.itemTemplate.itemName}  x{data.itemCount}  (id: {id})");
        }

        Debug.Log(sb.ToString());
    }

    // 切页接口（可以由 UI Toggle 调用）
    public void SwitchPage(ItemType type)
    {
        currentShowType = type;
        RefreshView();
    }
}