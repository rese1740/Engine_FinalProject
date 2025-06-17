using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    public ItemData itemData;  // 이 슬롯의 아이템 데이터
    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.Space))
        {
            TryPurchase();
        }
    }

    void TryPurchase()
    {
        if (PlayerSO.Instance.gold >= itemData.price)
        {
            PlayerSO.Instance.gold -= itemData.price;

            if (itemData.prefab != null)
            {
                // 원하는 위치에 아이템 프리팹 생성 (예: 플레이어 옆)
                Vector3 spawnPosition = gameObject.transform.position ;
                Instantiate(itemData.prefab, spawnPosition, Quaternion.identity);

                Debug.Log($"'{itemData.ItemName}' 아이템 생성 완료!");
            }
            else
            {
                Debug.LogWarning($"'{itemData.ItemName}' 아이템은 프리팹이 설정되지 않았습니다.");
            }

            Destroy(gameObject); // 상점에서 아이템 제거
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("상점 아이템 범위에 들어옴");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
