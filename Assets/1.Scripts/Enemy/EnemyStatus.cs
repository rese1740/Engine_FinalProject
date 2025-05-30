using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(Animator))]
public class EnemyStatus : MonoBehaviour
{
    [Header("Enemy Stats")]
    public EnemySO enemyData;
    public float maxHp = 20f;
    public float currentHp;
    public float moveSpeed = 2f;

    [Header("Components")]
    private Animator animator;
    private Rigidbody2D rb;
    private bool isDead = false;

    void Start()
    {
        StatReload();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
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

    void Die()
    {
        isDead = true;
        animator.SetTrigger("Die");
        rb.velocity = Vector2.zero;

        // Collider를 비활성화해서 더 이상 충돌하지 않게 처리
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        // 죽은 후 사라짐 (2초 뒤 파괴)
        Destroy(gameObject, 2f);
    }
}
