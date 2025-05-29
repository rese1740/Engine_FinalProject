using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public Image iconImage;
    private ItemData currentItem;
    private bool slotEnabled = false;

    public ItemTooltip tooltip; 

   

    private void Update()
    {
        if(slotEnabled)
        {
            iconImage.enabled = true;
        }
        else
        {
            iconImage.enabled = false;
        }
    }

    public void SetItem(ItemData item)
    {
        currentItem = item;

        if (item != null && item.icon != null)
        {
            iconImage.sprite = item.icon;
            slotEnabled = true;
        }
        else
        {
            slotEnabled = false;
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
            tooltip.ShowTooltip(currentItem, Input.mousePosition, this);
        }
    }
}
