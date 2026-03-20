using UnityEngine;
using UnityEngine.InputSystem;

public class Campfire : MonoBehaviour
{
    [Header("Detection")]
    public LayerMask playerLayer;
    
    [Header("Identification")]
    public string campfireId;
    
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
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            isPlayerInRange = false;
            player = null; // 清除玩家引用
        }
    }
    
    private void Interact()
    {
        if (!isActivated)
        {
            isActivated = true;
        }
        
        // 保存游戏
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.SaveGame(campfireId);
        }
        
        Debug.Log("已与篝火交互并保存游戏");
    }
}