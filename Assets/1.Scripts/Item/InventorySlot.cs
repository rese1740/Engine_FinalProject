using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;


public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public Image iconImage;
    public ItemTooltip tooltip; // 슬롯마다 자신의 툴팁 연결
    private ItemData currentItem;
    private bool slotEnabled = false;

    private void Update()
    {
        iconImage.enabled = slotEnabled;
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
        GameObject playerObj = GameObject.FindWithTag("Player");
        Instantiate(currentItem.prefab, playerObj.transform.position, Quaternion.identity);
        currentItem = null;
        iconImage.sprite = null;
        slotEnabled = false;
        tooltip.HideTooltip(); 
    }

    public bool HasItem()
    {
        return currentItem != null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentItem != null)
        {
            Vector3 pos = gameObject.transform.position;
            pos.x += 50f; 
            pos.y += 150f;  

            tooltip.ShowTooltip(currentItem, pos, this);
        }
    }
}
