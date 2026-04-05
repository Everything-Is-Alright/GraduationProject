using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Detection")]
    public LayerMask playerLayer;
    
    public float moveDistance = 2f; // 移动距离
    public float moveSpeed = 2f; // 移动速度
    public float cooldownTime = 2f; // 冷却时间
    
    [Header("References")]
    public Transform platformTransform; // 平台的Transform
    
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool isMoving = false;
    private bool isOnCooldown = false;
    private float cooldownTimer = 0f;
    private int directionCounter = 1; // 方向计数器，1为向上，-1为向下
    private GameObject currentPlayer; // 当前在平台上的玩家
    
    private void Start()
    {
        // 记录初始位置
        startPosition = platformTransform.position;
    }
    
    private void Update()
    {
        // 处理移动
        if (isMoving)
        {
            MovePlatform();
        }
        
        // 处理冷却
        if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                isOnCooldown = false;
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0 && !isMoving && !isOnCooldown)
        {
            // 记录当前玩家
            currentPlayer = other.gameObject;
            
            // 计算目标位置
            CalculateTargetPosition();
            // 触发平台移动
            StartMoving();
        }
    }
    
    private void CalculateTargetPosition()
    {
        // 根据方向计数器计算目标位置
        if (directionCounter == 1)
        {
            // 向上移动
            targetPosition = platformTransform.position + new Vector3(0f, moveDistance, 0f);
        }
        else
        {
            // 向下移动
            targetPosition = platformTransform.position - new Vector3(0f, moveDistance, 0f);
        }
    }
    
    private void StartMoving()
    {
        isMoving = true;
        
        // 如果有玩家，将其设置为平台的子物体并禁用移动
        if (currentPlayer != null)
        {
            // 禁用玩家移动
            Player playerScript = currentPlayer.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.SetMovementEnabled(false);
            }
            
            // 将玩家设置为平台的子物体
            currentPlayer.transform.parent = platformTransform;
        }
        
        Debug.Log("平台开始移动");
    }
    
    private void MovePlatform()
    {
        // 向目标位置移动
        platformTransform.position = Vector3.MoveTowards(platformTransform.position, targetPosition, moveSpeed * Time.deltaTime);
        
        // 检查是否到达目标位置
        if (Vector3.Distance(platformTransform.position, targetPosition) < 0.01f)
        {
            // 到达目标位置
            platformTransform.position = targetPosition;
            isMoving = false;
            
            // 切换方向
            directionCounter *= -1;
            
            // 如果有玩家，将其从平台子物体中移除并重新启用移动
            if (currentPlayer != null)
            {
                // 将玩家从平台子物体中移除
                currentPlayer.transform.parent = null;
                
                // 重新启用玩家移动
                Player playerScript = currentPlayer.GetComponent<Player>();
                if (playerScript != null)
                {
                    playerScript.SetMovementEnabled(true);
                }
                
                // 清空当前玩家引用
                currentPlayer = null;
            }
            
            // 开始冷却
            StartCooldown();
            
            Debug.Log("平台移动完成");
        }
    }
    
    private void StartCooldown()
    {
        isOnCooldown = true;
        cooldownTimer = cooldownTime;
    }
}