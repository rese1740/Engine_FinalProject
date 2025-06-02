using TMPro;
using UnityEngine;

public class Item : MonoBehaviour
{

    [Header("Info")]
    private bool trigger = false;

    [Header("Component")]
    public ItemData itemData;
    public TextMeshProUGUI titleTxt;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && trigger)
        {
            if (itemData.ItemType == ItemType.Artifact && !InventoryUI.Instance.HasActiveItem())
            {
                Destroy(gameObject);
                PickupItem();
            }
        }
    }


    void ReloadUI()
    {
        titleTxt.text = itemData.ItemName;
    }

    public void PickupItem()
    {
        InventoryUI.Instance.AddItemToSlot(itemData);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ReloadUI();
            titleTxt.gameObject.SetActive(true);
            trigger = true;
        }
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            titleTxt.gameObject.SetActive(false);
            trigger = false;
        }
    }
}
