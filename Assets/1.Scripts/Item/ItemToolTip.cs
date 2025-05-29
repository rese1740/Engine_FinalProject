using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemTooltip : MonoBehaviour
{
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI descriptionText;
    public Image iconImage;
    private InventorySlot currentSlot;

    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        gameObject.SetActive(false);
    }

    public void ShowTooltip(ItemData item, Vector2 position, InventorySlot fromSlot)
    {
        currentSlot = fromSlot;

        itemNameText.text = item.ItemName;
        descriptionText.text = item.description;
        iconImage.sprite = item.icon;
        iconImage.enabled = item.icon != null;

        rectTransform.position = position;
        gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        currentSlot = null;
        gameObject.SetActive(false);
    }

    public bool IsShowingForSlot(InventorySlot slot)
    {
        return currentSlot == slot;
    }
}
