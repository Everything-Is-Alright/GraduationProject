using UnityEngine;

public class PassTrigger : MonoBehaviour
{
    [Header("检测设置")]
    public LayerMask playerLayer;

    [Header("交互UI")]
    public GameObject interactUI;

    [Header("通关条件")]
    [Tooltip("勾选后，必须携带指定物品才能通关")]
    public bool isRequireItem = false;
    [Tooltip("通关需要的特定物品（仅isRequireItem勾选时生效）")]
    public Item requiredItem;

    // 缓存玩家，避免每帧FindObjectOfType（优化性能）
    private Player _cachedPlayer;
    private bool isPlayerInRange = false;

    private void Start()
    {
        if (interactUI != null) interactUI.SetActive(false);
    }

    private void Update()
    {
        if (!isPlayerInRange || _cachedPlayer == null) return;

        // 按E触发通关逻辑
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryPassLevel();
        }
    }

    /// <summary>
    /// 尝试通关：判断是否需要物品 + 物品是否足够
    /// </summary>
    private void TryPassLevel()
    {
        // 情况1：不需要物品 → 直接通关
        if (!isRequireItem)
        {
            LevelManager.Instance?.LoadNextLevel();
            return;
        }

        // 情况2：需要物品，但未配置物品 → 报错提示
        if (requiredItem == null)
        {
            Debug.LogWarning("通关触发器：已勾选需要物品，但未指定【requiredItem】！");
            return;
        }

        if (PackageManager.GetItemCountStatic(requiredItem.itemId) >= 1)
        {
            Debug.Log("持有通关物品：" + requiredItem.itemName + "，成功通关！");
            LevelManager.Instance?.LoadNextLevel();
        }
        else
        {
            Debug.Log("缺少通关物品：" + requiredItem.itemName + "，无法通关！");
            // 这里可以后续添加UI提示（比如弹窗文字）
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            isPlayerInRange = true;
            _cachedPlayer = other.GetComponent<Player>();
            if (interactUI != null) interactUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            isPlayerInRange = false;
            _cachedPlayer = null;
            if (interactUI != null) interactUI.SetActive(false);
        }
    }
}