using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicBoss : MonoBehaviour
{
    [Header("Boss Stats")]
    public float maxHealth = 3;
    private float currentHealth;

    [Header("Setting")]
    public Transform startPos;

    [Header("Componets")]
    public GameObject player;
    public Slider bossHPSlider;

    [Header("Pattern 1")]
    public Transform[] AttackPos;
    public GameObject attackPrefab;
    public float attackForce = 10f; // ���� ���� ������ ��

    void Start()
    {
        currentHealth = maxHealth;
        StartCoroutine(Attack1());
    }
    private void Update()
    {
        bossHPSlider.value = currentHealth;
    }
    public void TakeDamage()
    {
        Debug.Log("�޾Ҿ��");
        currentHealth -= 1;
        StartCoroutine(Damage());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject); 
    }

    IEnumerator Damage()
    {
        PlayerStatus.Instance.isControlLocked = true;
        PlayerStatus.Instance.StartDance();
        yield return new WaitForSeconds(3f);
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.transform.position = startPos.position;
        yield return new WaitForSeconds(1f);
        PlayerStatus.Instance.StopDance();
        PlayerStatus.Instance.isControlLocked = false;
    }

    IEnumerator Attack1()
    {
        while (true)
        {
            int randomPos = Random.Range(0, AttackPos.Length);

            GameObject magicBall = Instantiate(attackPrefab, AttackPos[randomPos].position, Quaternion.identity);

            Rigidbody2D rb = magicBall.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(Vector2.down * attackForce, ForceMode2D.Impulse);
            }

            yield return new WaitForSeconds(3f);
        }
    }
}
