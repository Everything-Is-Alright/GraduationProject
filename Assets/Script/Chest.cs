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
    public GameObject interactUI; // 交互提示界面
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

    private void Start()
    {
        chestAnimator = GetComponentInChildren<Animator>();

        if (chestPanel != null)
        {
            chestPanel.SetActive(false);
        }
        
        if (interactUI != null)
        {
            interactUI.SetActive(false);
        }
    }
    
    private void OnEnable()
    {
        // 监听玩家重生事件
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.OnPlayerRespawn += OnPlayerRespawn;
        }
    }
    
    private void OnDisable()
    {
        // 移除监听
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.OnPlayerRespawn -= OnPlayerRespawn;
        }
    }
    
    private void OnPlayerRespawn()
    {
        // 玩家重生时，清除旧引用
        player = null;
        isPlayerInRange = false;
        
        // 确保面板关闭
        if (isPanelOpen)
        {
            ClosePanel();
        }
    }

    void Update()
    {
        if (isPlayerInRange && player != null)
        {
            // 直接使用Input.GetKeyDown检测E键，不受玩家输入系统状态影响
            if (Input.GetKeyDown(KeyCode.E))
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
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            isPlayerInRange = true;
            player = other.GetComponent<Player>(); // 自动获取玩家引用
            Debug.Log("按E打开宝箱");
            
            // 显示交互提示界面
            if (interactUI != null)
            {
                interactUI.SetActive(true);
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            isPlayerInRange = false;
            player = null; // 清除玩家引用

            if (isPanelOpen)
            {
                ClosePanel();
            }
            
            // 隐藏交互提示界面
            if (interactUI != null)
            {
                interactUI.SetActive(false);
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
        
        // 禁用玩家操作
        DisablePlayerMovement();

        RefreshRewardSlots();
    }

    private void ClosePanel()
    {
        if (chestPanel == null) return;

        chestPanel.SetActive(false);
        isPanelOpen = false;
        
        // 启用玩家操作
        EnablePlayerMovement();
    }
    
    private void DisablePlayerMovement()
    {
        // 查找玩家并禁用其移动
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            player.SetMovementEnabled(false);
        }
    }
    
    private void EnablePlayerMovement()
    {
        // 查找玩家并启用其移动
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            player.SetMovementEnabled(true);
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
            
            // 延迟刷新，避免与Slot点击事件冲突
            Invoke("RefreshRewardSlots", 0.1f);
            
            // 刷新界面
            // RefreshRewardSlots();
            
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
