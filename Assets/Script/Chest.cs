using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class Chest : MonoBehaviour
{
    [Header("Detection")]
    public LayerMask playerLayer;

    [Header("UI")]
    public GameObject chestPanel;
    public Transform slotContainer;
    public Slot slotPrefab;

    [Header("Rewards")]
    public List<Item> rewardItems = new List<Item>();

    [Header("References")]
    public Player player;

    private bool isPlayerInRange = false;
    private bool isChestOpened = false;
    private bool isPanelOpen = false;
    private bool rewardsCollected = false;
    private Animator chestAnimator;

    private List<Slot> currentSlots = new List<Slot>();

    void Start()
    {
        chestAnimator = GetComponentInChildren<Animator>();

        if (chestPanel != null)
        {
            chestPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (isPlayerInRange && player != null &&
            player.input.Player.Action.WasPressedThisFrame())
        {
            if (!isChestOpened)
            {
                OpenChest();
            }
            else if (isPanelOpen)
            {
                ClosePanel();
            }
            else
            {
                OpenPanel();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            isPlayerInRange = true;
            Debug.Log("按E打开宝箱");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            isPlayerInRange = false;

            if (isPanelOpen)
            {
                ClosePanel();
            }
        }
    }

    private void OpenChest()
    {
        isChestOpened = true;

        if (chestAnimator != null)
        {
            chestAnimator.SetBool("IsTriggered", true);
            Debug.Log("宝箱打开动画已播放");
        }
        else
        {
            Debug.LogWarning("未找到宝箱Animator组件");
        }

        OpenPanel();
    }

    private void OpenPanel()
    {
        if (chestPanel == null)
        {
            Debug.LogWarning("未设置宝箱界面");
            return;
        }

        chestPanel.SetActive(true);
        isPanelOpen = true;
        
        // 禁用玩家背包打开
        if (player != null)
        {
            player.SetPackageOpenEnabled(false);
        }

        RefreshRewardSlots();
    }

    private void ClosePanel()
    {
        if (chestPanel == null) return;

        chestPanel.SetActive(false);
        isPanelOpen = false;
        
        // 启用玩家背包打开
        if (player != null)
        {
            player.SetPackageOpenEnabled(true);
        }
    }

    public void ForceClosePanel()
    {
        ClosePanel();
    }

    private void RefreshRewardSlots()
    {
        ClearAllSlots();

        if (slotContainer == null || slotPrefab == null)
        {
            Debug.LogWarning("未设置Slot容器或预制体");
            return;
        }

        Dictionary<Item, int> itemCountDict = new Dictionary<Item, int>();

        foreach (Item item in rewardItems)
        {
            if (item == null) continue;

            if (itemCountDict.ContainsKey(item))
            {
                itemCountDict[item]++;
            }
            else
            {
                itemCountDict[item] = 1;
            }
        }

        foreach (var kvp in itemCountDict)
        {
            CreateSlot(kvp.Key, kvp.Value);
        }
    }

    private void CreateSlot(Item item, int count)
    {
        Slot newSlot = Instantiate(slotPrefab, slotContainer);
        newSlot.slotItem = item;

        if (newSlot.slotImage != null)
        {
            newSlot.slotImage.sprite = item.itemImage;
            newSlot.slotImage.enabled = true;
        }

        if (newSlot.slotNum != null)
        {
            newSlot.slotNum.text = count > 1 ? count.ToString() : "";
        }

        currentSlots.Add(newSlot);
    }

    private void ClearAllSlots()
    {
        foreach (Slot slot in currentSlots)
        {
            if (slot != null)
            {
                Destroy(slot.gameObject);
            }
        }
        currentSlots.Clear();
    }

    public void CollectAllRewards()
    {
        if (rewardsCollected) return;

        Dictionary<Item, int> itemCountDict = new Dictionary<Item, int>();

        foreach (Item item in rewardItems)
        {
            if (item == null) continue;

            if (itemCountDict.ContainsKey(item))
            {
                itemCountDict[item]++;
            }
            else
            {
                itemCountDict[item] = 1;
            }
        }

        foreach (var kvp in itemCountDict)
        {
            for (int i = 0; i < kvp.Value; i++)
            {
                PackageManager.AddItemToPackage(kvp.Key);
            }
        }

        rewardItems.Clear();
        rewardsCollected = true;
        RefreshRewardSlots();
        Debug.Log("已收集所有奖励物品");
    }
    
    public void CollectSingleItem(Item item)
    {
        if (item == null) return;
        
        // 从宝箱中移除该物品
        if (rewardItems.Contains(item))
        {
            rewardItems.Remove(item);
            
            // 添加到玩家背包
            PackageManager.AddItemToPackage(item);
            
            // 刷新界面
            RefreshRewardSlots();
            
            // 检查是否所有物品都已收集
            if (rewardItems.Count == 0)
            {
                rewardsCollected = true;
            }
            
            Debug.Log($"已收集物品: {item.itemName}");
        }
    }

    public void AddRewardItem(Item item)
    {
        if (item == null) return;
        rewardItems.Add(item);
    }

    public void ClearRewards()
    {
        rewardItems.Clear();
    }

    public bool HasRewards()
    {
        return rewardItems.Count > 0;
    }

    public bool IsRewardsCollected()
    {
        return rewardsCollected;
    }
}
