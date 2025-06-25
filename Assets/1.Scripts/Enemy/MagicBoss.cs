using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    private Animator animator;

    [Header("Pattern 1")]
    public Transform[] AttackPos;
    public GameObject attackPrefab;
    public float attackForce = 10f; 

    [Header("Pattern 2")]
    public Transform firePoint; 
    public int bulletCount = 5;
    public float spreadAngle = 45f; 
    public float attack2Interval = 5f;

    [Header("Pattern 3")]
    public GameObject homingPrefab;
    public float homingSpeed = 5f;
    public float homingDuration = 5f;
    public float attack3Interval = 6f;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        StartCoroutine(Attack1());
        StartCoroutine(Attack2());
        StartCoroutine(Attack3());
    }
    private void Update()
    {
        bossHPSlider.value = currentHealth;
    }
    public void TakeDamage()
    {
        Debug.Log("받았어요");
        currentHealth -= 1;
        StartCoroutine(Damage());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        int StageIndex = SceneManager.GetActiveScene().buildIndex;
        int NextStageIndex = StageIndex += 1;
        DataBaseManager.Instance.currenStage++;

        int curretStage = PlayerPrefs.GetInt("curretStage", 0);

        curretStage++;

        PlayerPrefs.SetInt("curretStage", curretStage);
        Destroy(gameObject);
        SceneManager.LoadScene(NextStageIndex);
    }

    IEnumerator Damage()
    {
        PlayerStatus.Instance.isControlLocked = true;
        PlayerStatus.Instance.StartDance();
        yield return new WaitForSeconds(2f);
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(1f);
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
            animator.SetTrigger("Attack");
            int randomPos = Random.Range(0, AttackPos.Length);

            GameObject magicBall = Instantiate(attackPrefab, AttackPos[randomPos].position, Quaternion.identity);

            Rigidbody2D rb = magicBall.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(Vector2.down * attackForce, ForceMode2D.Impulse);
            }

            yield return new WaitForSeconds(2f);
        }
    }

    IEnumerator Attack2()
    {
        while (true)
        {
            animator.SetTrigger("Attack");
            float startAngle = -spreadAngle / 2f;
            float angleStep = spreadAngle / (bulletCount - 1);

            for (int i = 0; i < bulletCount; i++)
            {
                float angle = startAngle + (angleStep * i);
                Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.down;

                GameObject bullet = Instantiate(attackPrefab, firePoint.position, Quaternion.identity);
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.AddForce(direction.normalized * attackForce, ForceMode2D.Impulse);
                }
            }

            yield return new WaitForSeconds(attack2Interval);
        }
    }

    IEnumerator Attack3()
    {
        while (true)
        {
            animator.SetTrigger("Attack");

            GameObject homing = Instantiate(homingPrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = homing.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                StartCoroutine(HomingMove(rb));
            }

            yield return new WaitForSeconds(attack3Interval);
        }
    }

    IEnumerator HomingMove(Rigidbody2D rb)
    {
        float timer = 0f;

        while (timer < homingDuration && player != null)
        {
            Vector2 direction = (player.transform.position - rb.transform.position).normalized;
            rb.velocity = direction * homingSpeed;

            timer += Time.deltaTime;
            yield return null;
        }
        rb.velocity = Vector2.zero;

        Animator anim = rb.GetComponent<Animator>();
        anim.SetTrigger("Explosion");

        yield return new WaitForSeconds(0.5f); 
        Destroy(rb.gameObject, 0.5f); 
    }

}
