using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpText : MonoBehaviour
{
    public GameObject textObject; // 월드에 위치한 텍스트 오브젝트 (TextMeshPro - Text)

    private void Start()
    {
        if (textObject != null)
        {
            textObject.SetActive(false); // 시작 시 텍스트 숨김
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (textObject != null)
                textObject.SetActive(true); // 닿으면 보이게
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (textObject != null)
                textObject.SetActive(false); // 떨어지면 숨김
        }
    }
}
