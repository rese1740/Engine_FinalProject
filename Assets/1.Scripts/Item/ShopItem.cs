using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    public ItemData itemData;  // �� ������ ������ ������
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
                // ���ϴ� ��ġ�� ������ ������ ���� (��: �÷��̾� ��)
                Vector3 spawnPosition = gameObject.transform.position ;
                Instantiate(itemData.prefab, spawnPosition, Quaternion.identity);

                Debug.Log($"'{itemData.ItemName}' ������ ���� �Ϸ�!");
            }
            else
            {
                Debug.LogWarning($"'{itemData.ItemName}' �������� �������� �������� �ʾҽ��ϴ�.");
            }

            Destroy(gameObject); // �������� ������ ����
        }
        else
        {
            Debug.Log("��尡 �����մϴ�.");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("���� ������ ������ ����");
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
