using UnityEngine;

public class NPC : MonoBehaviour
{
    [Header("Detection")]
    public LayerMask playerLayer;
    
    [Header("Quest")]
    public string questId; // 关联的任务ID
    
    [Header("UI")]
    public GameObject interactUI; // 交互提示界面
    
    private bool isPlayerInRange = false;
    
    private void Start()
    {
        if (interactUI != null)
        {
            interactUI.SetActive(false);
        }
    }
    
    private void Update()
    {
        if (isPlayerInRange)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            isPlayerInRange = true;
            Debug.Log("按E与NPC交互");
            
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
            
            // 隐藏交互提示界面
            if (interactUI != null)
            {
                interactUI.SetActive(false);
            }
        }
    }
    
}

