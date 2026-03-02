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
}
