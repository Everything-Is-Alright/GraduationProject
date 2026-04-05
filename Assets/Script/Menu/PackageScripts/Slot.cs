using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class Slot : MonoBehaviour
{
    private static Dictionary<GameObject, Slot> slotCache = new Dictionary<GameObject, Slot>();

    public Item slotItem;
    public Image slotImage;
    public TextMeshProUGUI slotNum;

    public enum EquipmentSlotType { None, WeaponSlot, ArmorSlot, AccessoriesSlot }
    public EquipmentSlotType equipSlotType = EquipmentSlotType.None;
    
    private void Awake()
    {
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnSlotClick);
        }
        else
        {
            Debug.LogWarning("Slot预制体缺少Button组件，无法添加点击事件");
        }
    }
    
    private void OnSlotClick()
    {
        if (slotItem != null)
        {
            // Debug显示点击的物品名称
            Debug.Log("点击的物品: " + slotItem.itemName);
            
            // 尝试获取PackageTextManager并更新文本
            PackageTextManager textManager = FindObjectOfType<PackageTextManager>();
            if (textManager != null)
            {
                textManager.UpdateItemText(slotItem);
            }
            
            // 检查是否是宝箱中的物品
            Chest chest = GetComponentInParent<Chest>();
            if (chest != null)
            {
                chest.CollectSingleItem(slotItem);
            }
        }
    }
}
