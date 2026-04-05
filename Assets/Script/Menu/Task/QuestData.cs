using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Package/New Quest")]
public class QuestData : ScriptableObject
{
    [Header("任务基础信息")]
    public string questName;
    [TextArea] public string questDesc;

    [Header("任务要求：任务道具")]
    public Item targetTaskItem;

    [Header("任务完成奖励：发放给玩家的道具")]
    public Item rewardItem;
}