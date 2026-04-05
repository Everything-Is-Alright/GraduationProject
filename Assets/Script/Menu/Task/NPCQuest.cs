using UnityEngine;

public class NPCQuest : MonoBehaviour
{
    [Header("Detection")]
    public LayerMask playerLayer;

    [Header("UI")]
    public GameObject interactUI; // 交互提示UI（仅范围内显示）

    [Header("Dialogue")]
    public DialogueData beforeQuestDialogue;  // 任务未完成对话
    public DialogueData afterCompleteDialogue;// 任务完成对话

    [Header("Quest")]
    public QuestData questToGive;

    // 私有变量（和你的Chest完全一致）
    private bool isPlayerInRange = false;
    private Player player;
    private bool questAccepted = false;

    private void Start()
    {
        // 初始隐藏交互提示
        if (interactUI != null)
            interactUI.SetActive(false);
    }

    void Update()
    {
        if (isPlayerInRange && player != null && Input.GetKeyDown(KeyCode.E))
        {
            HandleNPCInteract();
        }
    }

    // NPC核心交互逻辑
    private void HandleNPCInteract()
    {
        // 防止对话期间重复触发
        if (DialogueManager.Instance.isDialogueActive)
            return;

        // 1. 任务已完成 → 播放完成对话
        if (QuestManager.Instance.questCompleted)
        {
            DialogueManager.Instance.StartDialogue(afterCompleteDialogue);
            return;
        }

        // 2. 已接任务 + 背包有任务物品 → 交付完成
        if (questAccepted && QuestManager.Instance.HasQuestItemInPackage())
        {
            QuestManager.Instance.CompleteQuestByPackage();
            DialogueManager.Instance.StartDialogue(afterCompleteDialogue);
            return;
        }

        // 3. 未完成 → 播放任务对话
        DialogueManager.Instance.StartDialogue(beforeQuestDialogue);

        // 首次对话结束 → 接任务
        if (!questAccepted && questToGive != null)
        {
            questAccepted = true;
            QuestManager.Instance.AcceptQuest(questToGive);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 玩家进入范围：标记+获取引用+显示提示UI
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            isPlayerInRange = true;
            player = other.GetComponent<Player>();

            if (interactUI != null)
                interactUI.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        // 玩家离开范围：取消标记+清空引用+隐藏提示UI
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            isPlayerInRange = false;
            player = null;

            if (interactUI != null)
                interactUI.SetActive(false);
        }
    }
}