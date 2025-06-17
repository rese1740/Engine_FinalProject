using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public List<ItemData> allItems;
    public int itemCountToShow = 4;

    public GameObject shopItemPrefab;
    public Transform[] itemSpawnPoints; // �����۵��� ��Ÿ�� ��ġ 4��

    private List<GameObject> currentSlots = new();

    void Start()
    {
        GenerateShopItems();
    }

    void GenerateShopItems()
    {
        // ���� ���� ����
        foreach (var slot in currentSlots)
        {
            Destroy(slot);
        }
        currentSlots.Clear();

        List<ItemData> tempList = new(allItems);

        for (int i = 0; i < itemCountToShow; i++)
        {
            if (tempList.Count == 0 || i >= itemSpawnPoints.Length)
                break;

            int randIndex = Random.Range(0, tempList.Count);
            ItemData selectedItem = tempList[randIndex];
            tempList.RemoveAt(randIndex);

            GameObject slot = Instantiate(shopItemPrefab, itemSpawnPoints[i].position, Quaternion.identity);
            slot.name = $"ShopItem_{selectedItem.ItemName}";

            ShopItem shopItem = slot.GetComponentInChildren<ShopItem>();
           
            if (shopItem != null)
            {
                shopItem.itemData = selectedItem;
            }
            // ���� ��� ����
            Transform iconTransform = slot.transform.Find("Icon");
            if (iconTransform != null)
            {
                SpriteRenderer sr = iconTransform.GetComponent<SpriteRenderer>();
                if (sr != null)
                    sr.sprite = selectedItem.icon;
            }

            TextMeshProUGUI priceText = slot.GetComponentInChildren<TextMeshProUGUI>();
            if (priceText != null)
                priceText.text = selectedItem.price + " G";
            
            currentSlots.Add(slot);
        }
    }
}
