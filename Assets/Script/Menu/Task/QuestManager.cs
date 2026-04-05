using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [Header("当前任务配置")]
    public QuestData currentQuest;      // 当前接取的任务
    public bool questCompleted = false;// 任务是否完成

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else { Instance = this; DontDestroyOnLoad(gameObject); }
    }

    // 接取任务
    public void AcceptQuest(QuestData quest)
    {
        if (currentQuest == null)
        {
            currentQuest = quest;
            questCompleted = false;
            Debug.Log("接取任务：" + quest.questName);
        }
    }

    public bool HasQuestItemInPackage()
    {
        // 无任务 / 无物品配置 → 不通过
        if (currentQuest == null || currentQuest.targetTaskItem == null)
            return false;

        // 直接调用你的背包管理器，检查是否拥有该物品
        return PackageManager.GetItemCountStatic(currentQuest.targetTaskItem.itemId) >= 1;
    }

    public void CompleteQuestByPackage()
    {
        if (!HasQuestItemInPackage() || questCompleted) return;

        // 扣除背包里的任务物品（交付给NPC）
        PackageManager.RemoveItemFromPackage(currentQuest.targetTaskItem.itemId, 1);

        if (currentQuest.rewardItem != null)
        {
            PackageManager.AddItemToPackage(currentQuest.rewardItem);
            Debug.Log("任务奖励已发放：" + currentQuest.rewardItem.itemName);
        }

        questCompleted = true;
        Debug.Log("任务完成！已交付：" + currentQuest.targetTaskItem.itemName);
    }

    // 重置任务（可选，用于后续任务）
    public void ResetQuest()
    {
        currentQuest = null;
        questCompleted = false;
    }
}