using UnityEngine;
using UnityEngine.InputSystem;

public class Door : MonoBehaviour
{
    [Header("Detection")]
    public LayerMask playerLayer;
    
    [Header("References")]
    public Player player;
    public GameObject closedDoor; // 关闭的门 GameObject
    public GameObject openDoor;   // 打开的门 GameObject
    
    [Header("State")]
    public bool isOpen = false;
    
    private bool isPlayerInRange = false;
    
    private void Start()
    {
        // 初始化门的状态
        UpdateDoorState();
        
        // 尝试查找玩家
        FindPlayer();
    }
    
    private void Update()
    {
        // 如果玩家引用为null，尝试查找玩家
        if (player == null)
        {
            FindPlayer();
        }
        
        if (isPlayerInRange && player != null && !isOpen &&
            player.input != null &&
            player.input.Player.Action.WasPressedThisFrame())
        {
            OpenDoor();
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            isPlayerInRange = true;
            player = other.GetComponent<Player>(); // 自动获取玩家引用
            if (!isOpen)
            {
                Debug.Log("按E打开门");
            }
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
    
    private void OpenDoor()
    {
        isOpen = true;
        UpdateDoorState();
        Debug.Log("门已打开");
    }
    
    private void UpdateDoorState()
    {
        // 更新门的显示状态
        if (closedDoor != null)
        {
            closedDoor.SetActive(!isOpen);
        }
        
        if (openDoor != null)
        {
            openDoor.SetActive(isOpen);
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
        
        // 尝试全局查找玩家
        FindPlayer();
    }
    
    private void FindPlayer()
    {
        // 尝试全局查找玩家
        var playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.GetComponent<Player>();
        }
    }
}