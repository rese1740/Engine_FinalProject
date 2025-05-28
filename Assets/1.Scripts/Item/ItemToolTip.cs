using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemTooltip : MonoBehaviour
{
    public Text itemNameText;
    public Text descriptionText;
    public Image iconImage;

    private RectTransform rectTransform;


    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        gameObject.SetActive(false);
    }

    public void ShowTooltip(ItemData item, Vector2 position)
    {
        itemNameText.text = item.ItemName;
        descriptionText.text = item.description;
        iconImage.sprite = item.icon;
        iconImage.enabled = item.icon != null;

        rectTransform.position = position;
        gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}
