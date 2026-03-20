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
            Chest chest = GetComponentInParent<Chest>();
            if (chest != null)
            {
                chest.CollectSingleItem(slotItem);
            }
        }
    }
}
