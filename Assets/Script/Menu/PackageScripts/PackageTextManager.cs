using TMPro;
using UnityEngine;

public class PackageTextManager : MonoBehaviour
{
    [Header("Text References")]
    public TextMeshProUGUI nameText; // 显示物品名称的文本
    public TextMeshProUGUI introductionText; // 显示物品介绍的文本
    
    private void Start()
    {
        // 初始化文本为空
        ClearText();
    }
    
    // 更新文本显示
    public void UpdateItemText(Item item)
    {
        if (item != null)
        {
            // 更新物品名称
            if (nameText != null)
            {
                nameText.text = item.itemName;
            }
            
            // 更新物品介绍
            if (introductionText != null)
            {
                introductionText.text = item.itemInfo;
            }
        }
        else
        {
            // 如果物品为空，清空文本
            ClearText();
        }
    }
    
    // 清空文本
    public void ClearText()
    {
        if (nameText != null)
        {
            nameText.text = "";
        }
        
        if (introductionText != null)
        {
            introductionText.text = "";
        }
    }
}
