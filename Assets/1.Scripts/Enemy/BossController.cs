using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum BossState
{
    Idle,
    Chase,
    Attack,
    Attack2
}

public class BossController : MonoBehaviour
{
    public BossState currentState = BossState.Idle;
    public Transform player;
    public float chaseRange = 10f;
    public float chaseSpeed = 1.0f;
    public float attackRange = 3f;

    [Header("Boss Stats")]
    public float maxHealth = 100;
    private float currentHealth;

    [Header("Dash")]
    public float chargeTime = 3f;
    public float dashForce = 20f;
    private bool isDashing = false;
    public GhostManager ghost;

    [Header("Attack Cooldown")]
    public float dashCooldown = 2f;
    private float lastDashTime = -Mathf.Infinity;

    [Header("Component")]
    public Slider bossHPSlider;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        bossHPSlider.value = currentHealth;
        ghost.isGhosting = isDashing;
        if (isDashing) return;

        switch (currentState)
        {
            case BossState.Idle:
                HandleIdle();
                break;
            case BossState.Chase:
                HandleChase();
                break;
            case BossState.Attack:
                HandleAttack();
                break;
            case BossState.Attack2:
                HandleAttack2();
                break;
        }
    }

    void HandleIdle()
    {
        if (Vector2.Distance(transform.position, player.position) < chaseRange && !PlayerStatus.Instance.isDie)
            ChangeState(BossState.Chase);
    }

    void HandleChase()
    {
        Vector2 direction = (player.position - transform.position);

        if (direction == Vector2.zero) return;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            direction = new Vector2(Mathf.Sign(direction.x), 0);
        else
            direction = new Vector2(0, Mathf.Sign(direction.y));

        rb.velocity = direction * chaseSpeed;

        animator.SetFloat("moveX", direction.x);
        animator.SetFloat("moveY", direction.y);
        animator.SetBool("isCharging", isDashing);

        if (Vector2.Distance(transform.position, player.position) < attackRange && !PlayerStatus.Instance.isDie)
        {
            rb.velocity = Vector2.zero;
            ChangeState(BossState.Attack);
        }
    }

    void HandleAttack()
    {
        if (Time.time >= lastDashTime + dashCooldown)
        {
            StartCoroutine(DashCoroutine());
            lastDashTime = Time.time;
        }
        else
        {
            ChangeState(BossState.Idle);
            rb.velocity = Vector2.zero;
        }
    }

    void HandleAttack2()
    {
        ChangeState(BossState.Idle);
    }

    void ChangeState(BossState newState)
    {
        currentState = newState;
    }

    IEnumerator DashCoroutine()
    {
        isDashing = true;
        rb.velocity = Vector2.zero;

        spriteRenderer.color = Color.yellow;
        yield return new WaitForSeconds(0.5f);

        spriteRenderer.color = new Color(1f, 0.5f, 0f);
        yield return new WaitForSeconds(0.5f);

        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.5f);

        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * dashForce;

        yield return new WaitForSeconds(0.5f);
        rb.velocity = Vector2.zero;

        spriteRenderer.color = Color.white;
        isDashing = false;

        ChangeState(BossState.Idle);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log("데미지 받음");
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
}
