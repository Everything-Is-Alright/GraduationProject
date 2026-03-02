using UnityEngine;
using UnityEngine.UI;

public class ItemDragTarget : MonoBehaviour
{
    public ItemType acceptItemType;
    public Image targetImage;

    public void ReplaceSlotImage(Sprite newSprite)
    {
        targetImage.sprite = newSprite;
        targetImage.enabled = true;
        Color imageColor = targetImage.color;
        imageColor.a = 1f;
        targetImage.color = imageColor;
    }
}