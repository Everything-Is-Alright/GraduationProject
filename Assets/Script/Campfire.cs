using UnityEngine;
using UnityEngine.InputSystem;

public class Campfire : MonoBehaviour
{
    [Header("Detection")]
    public LayerMask playerLayer;
    
    [Header("Identification")]
    public string campfireId;
    
    [Header("UI")]
    public GameObject interactUI; // 交互提示界面
    
    [Header("References")]
    public Player player;
    
    public bool isActivated = false;
    
    private bool isPlayerInRange = false;
    
    private void Start()
    {
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.AddCampfire(this);
        }
        
        if (interactUI != null)
        {
            interactUI.SetActive(false);
        }
    }
    
    private void OnDestroy()
    {
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.RemoveCampfire(this);
        }
    }
    
    private void Update()
    {
        if (isPlayerInRange && player != null &&
            player.input != null &&
            player.input.Player.Action.WasPressedThisFrame())
        {
            Interact();
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            isPlayerInRange = true;
            player = other.GetComponent<Player>(); // 自动获取玩家引用
            Debug.Log("按E与篝火交互");
            
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
            
            // 隐藏交互提示界面
            if (interactUI != null)
            {
                interactUI.SetActive(false);
            }
        }
    }
    
    private void Interact()
    {
        if (!isActivated)
        {
            isActivated = true;
        }
        
        // 恢复玩家血量
        if (player != null)
        {
            EntityHealth playerHealth = player.GetComponent<EntityHealth>();
            if (playerHealth != null)
            {
                playerHealth.ResetHealth();
                Debug.Log("玩家血量已恢复满");
            }
        }
        
        // 保存游戏
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.SaveGame(campfireId);
        }
        
        Debug.Log("已与篝火交互并保存游戏");
    }
}