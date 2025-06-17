using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(Animator))]
public class EnemyStatus : MonoBehaviour
{
    [Header("Enemy Stats")]
    public EnemySO enemyData;
    public float maxHp = 20f;
    public float currentHp;
    public float moveSpeed = 2f;

    [Header("Combat Settings")]
    public float attackRange = 1.5f;
    public float attackDelay = 0.2f;
    public float attackCooldown = 1f;
    public GameObject attackHitboxPrefab; 
    public GameObject goldPrefab;
    public float spawnForce = 5f;

    public Transform hitboxSpawnPoint; 

    [Header("Components")]
    private Animator animator;
    private Rigidbody2D rb;
    private Transform player;
    private ProceduralRoom room;

    private bool isDead = false;
    private float lastAttackTime = -999f;
    public bool isAttacking = false;

    enum EnemyState { Idle, Chasing, Attacking }
    private EnemyState currentState = EnemyState.Chasing;

    void Start()
    {
        StatReload();
        room = GetComponentInParent<ProceduralRoom>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (isDead || player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        switch (currentState)
        {
            case EnemyState.Chasing:
                if (distanceToPlayer <= attackRange)
                {
                    animator.SetBool("move", false);
                    currentState = EnemyState.Attacking;
                    rb.velocity = Vector2.zero;
                    StartCoroutine(AttackAfterDelay());
                }
                else
                {
                    animator.SetBool("move",true);
                    Vector2 dir = (player.position - transform.position).normalized;
                    rb.velocity = dir * moveSpeed;

                    // 방향 전환 (왼쪽이면 -1, 오른쪽이면 1)
                    if (dir.x != 0)
                    {
                        Vector3 scale = transform.localScale;
                        scale.x = Mathf.Sign(dir.x) * Mathf.Abs(scale.x);
                        transform.localScale = scale;
                    }
                }
                break;

            case EnemyState.Attacking:
                rb.velocity = Vector2.zero;
                break;
        }
    }


    IEnumerator AttackAfterDelay()
    {
        if (Time.time - lastAttackTime < attackCooldown)
        {
            currentState = EnemyState.Chasing;
            yield break;
        }

        isAttacking = true;
        yield return new WaitForSeconds(attackDelay);

        animator.SetTrigger("Attack"); 
        lastAttackTime = Time.time;
    }

    public void SpawnAttackHitbox()
    {
        if (attackHitboxPrefab != null && hitboxSpawnPoint != null)
        {
            Instantiate(attackHitboxPrefab, hitboxSpawnPoint.position, Quaternion.identity);
        }

        isAttacking = false;
        currentState = EnemyState.Chasing;
    }

    void StatReload()
    {
        maxHp = enemyData.maxHp;
        currentHp = maxHp;
        moveSpeed = enemyData.moveSpeed;
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;
        currentHp -= damage;
        animator.SetTrigger("Hit");

        if (currentHp <= 0)
        {
            Die();
        }
    }


    private void SpawnGold()
    {
        int goldCount = Random.Range(1, enemyData.goldReward + 1);

        for (int i = 0; i < goldCount; i++)
        {
            GameObject gold = Instantiate(goldPrefab, transform.position, Quaternion.identity);

            Rigidbody2D rb = gold.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                // 튀어나갈 방향 계산
                Vector2 direction = GetRandomDirection();
                rb.AddForce(direction * spawnForce, ForceMode2D.Impulse);

            }
        }
    }
    private Vector2 GetRandomDirection()
    {
        Vector2[] directions = new Vector2[]
        {
            Vector2.left,
            Vector2.right,
            new Vector2(-1, 1).normalized,
            new Vector2(1, 1).normalized,
            new Vector2(-0.5f, 1).normalized,
            new Vector2(0.5f, 1).normalized
        };

        return directions[Random.Range(0, directions.Length)];
    }

    void Die()
    {
        room.enemyCount -= 1;
        isDead = true;
        animator.SetTrigger("Death");
        rb.velocity = Vector2.zero;
        SpawnGold();


        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        Destroy(gameObject, 2f);
    }
}
