using UnityEngine;

public class Trap : MonoBehaviour
{
    [Header("Detection")]
    public LayerMask playerLayer;
    
    [Header("Settings")]
    public bool oneTimeUse = true; // 是否一次性使用
    private bool isTriggered = false;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0 && !isTriggered)
        {
            // 获取玩家组件
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                // 触发陷阱，导致玩家死亡
                TriggerTrap(player);
            }
        }
    }
    
    private void TriggerTrap(Player player)
    {
        // 标记陷阱已触发
        if (oneTimeUse)
        {
            isTriggered = true;
        }
        
        // 调用玩家死亡方法
        if (player != null)
        {
            // 可以在这里添加陷阱触发的视觉或音效效果
            Debug.Log("陷阱触发，玩家死亡");
            
            // 调用玩家的死亡方法
            player.GetComponent<EntityHealth>().TakeDamage(9999, 0, transform); // 使用一个足够大的伤害值确保玩家死亡
        }
    }
}