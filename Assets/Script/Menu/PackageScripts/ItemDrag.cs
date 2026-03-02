using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragSlotImage : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private RectTransform imageRect;
    private RectTransform parentRect;
    private Vector2 offset;
    private Vector2 initialPosition;

    private Sprite dragSprite;
    private Image dragImage;

    private Slot parentSlot;

    void Awake()
    {
        imageRect = GetComponent<RectTransform>();
        dragImage = GetComponent<Image>();
        parentSlot = GetComponentInParent<Slot>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (dragImage != null)
        {
            dragSprite = dragImage.sprite;
        }
        else
        {
            dragSprite = null;
        }

        initialPosition = imageRect.anchoredPosition;
        parentRect = imageRect.parent as RectTransform;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRect,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint
        ))
        {
            offset = imageRect.anchoredPosition - localPoint;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRect,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint
        ))
        {
            imageRect.anchoredPosition = localPoint + offset;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (dragSprite == null || parentSlot == null || parentSlot.slotItem == null)
        {
            imageRect.anchoredPosition = initialPosition;
            offset = Vector2.zero;
            return;
        }

        ItemDragTarget hitSlot = GetHitItemDragTarget(eventData);
        if (hitSlot != null)
        {
            ItemType itemType = parentSlot.slotItem.itemType;
            ItemType targetAcceptType = hitSlot.acceptItemType;
            if (itemType == targetAcceptType)
            {
                hitSlot.ReplaceSlotImage(dragSprite);
            }
        }

        imageRect.anchoredPosition = initialPosition;
        offset = Vector2.zero;
        dragSprite = null;
    }

    private ItemDragTarget GetHitItemDragTarget(PointerEventData eventData)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = eventData.position;

        var raycastResults = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        foreach (var result in raycastResults)
        {
            ItemDragTarget slotTarget = result.gameObject.GetComponent<ItemDragTarget>();
            if (slotTarget == null)
            {
                slotTarget = result.gameObject.GetComponentInParent<ItemDragTarget>();
            }
            if (slotTarget != null)
            {
                return slotTarget;
            }
        }

        return null;
    }
}