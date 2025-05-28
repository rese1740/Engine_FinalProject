using TMPro;
using UnityEngine;

public class Item : MonoBehaviour
{

    [Header("Info")]
    private bool trigger = false;

    [Header("Component")]
    public ItemSO itemData;
    public TextMeshProUGUI titleTxt;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && trigger)
        {
            Destroy(gameObject);
        }
    }


    void ReloadUI()
    {
        titleTxt.text = itemData.ItemName;
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
