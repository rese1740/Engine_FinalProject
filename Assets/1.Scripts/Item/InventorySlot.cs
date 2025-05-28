using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image iconImage;
    private ItemData currentItem;

    public ItemTooltip tooltip; // Tooltip 오브젝트 참조 연결 필요

    private void Start()
    {
        iconImage.enabled = false;
    }

    public void SetItem(ItemData item)
    {
        currentItem = item;

        if (item != null && item.icon != null)
        {
            iconImage.sprite = item.icon;
            iconImage.enabled = true;
        }
        else
        {
            iconImage.enabled = false;
        }
    }

    public void ClearSlot()
    {
        currentItem = null;
        iconImage.sprite = null;
        iconImage.enabled = false;
    }

    public bool HasItem()
    {
        return currentItem != null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentItem != null)
        {
            tooltip.ShowTooltip(currentItem, Input.mousePosition);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentItem != null)
        {
            tooltip.ShowTooltip(currentItem, Input.mousePosition);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.HideTooltip();
    }
}
