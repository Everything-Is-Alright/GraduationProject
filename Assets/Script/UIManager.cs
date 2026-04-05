using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI Settings")]
    public GameObject menuUI; // 要控制的界面
    
    private void Start()
    {
        DontDestroyOnLoad(gameObject); 
    }
    
    private void Update()
    {
        // 检测玩家是否按下O键
        if (Input.GetKeyDown(KeyCode.O) && menuUI != null)
        {
            // 切换界面的激活状态
            bool newState = !menuUI.activeSelf;
            menuUI.SetActive(newState);
            
            // 根据界面状态控制玩家移动
            if (newState)
            {
                // 界面打开，禁用玩家移动
                DisablePlayerMovement();
            }
            else
            {
                // 界面关闭，启用玩家移动
                EnablePlayerMovement();
            }
        }
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
}