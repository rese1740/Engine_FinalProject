using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpText : MonoBehaviour
{
    public GameObject textObject; // ���忡 ��ġ�� �ؽ�Ʈ ������Ʈ (TextMeshPro - Text)

    private void Start()
    {
        if (textObject != null)
        {
            textObject.SetActive(false); // ���� �� �ؽ�Ʈ ����
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (textObject != null)
                textObject.SetActive(true); // ������ ���̰�
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (textObject != null)
                textObject.SetActive(false); // �������� ����
        }
    }
}
