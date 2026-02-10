using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item slotItem;
    public Image slotImage;
    public TextMeshProUGUI slotNum;

    public enum EquipmentSlotType { None, WeaponSlot, ArmorSlot, AccessoriesSlot }
    public EquipmentSlotType equipSlotType = EquipmentSlotType.None;
}
