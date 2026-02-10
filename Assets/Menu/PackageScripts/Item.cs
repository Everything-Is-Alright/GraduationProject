using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Package/New Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite itemImage;
    public int itemHeld;
    [TextArea]
    public string itemInfo;
    public bool isEquip;

    public ItemType itemType;
}
