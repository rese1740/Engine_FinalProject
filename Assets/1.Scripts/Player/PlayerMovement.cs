using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 moveInput;
    [Header("�ִϸ��̼�")]
    Animator animator;
    bool facingRight = true;
    public Light2D spotLight2D;

    [Header("PlayerStat")]
    public PlayerSO playerData;
    public float currentHp;
    public float maxHp;
    public float moveSpeed = 5f;
    public float damage = 5f;

    [Header("PlayerStat")]
    public float minRadius = 1f;   // HP 0�� �� �ּ� �ݰ�
    public float maxRadius = 5f;   // HP �ִ��� �� �ִ� �ݰ�

    [Header("Managers")]
    public StageManager stageManager;

    void Start()
    {
        PlayerStatReload();
        currentHp = maxHp;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        #region �̵�
        // �Է� �ޱ�
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput = moveInput.normalized;

        // �ִϸ��̼�: �ȱ� ����
        animator.SetBool("isMoving", moveInput.magnitude > 0);

        // ���� ��ȯ ó��
        if (moveInput.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveInput.x < 0 && facingRight)
        {
            Flip();
        }
        #endregion

        #region ��
        float hpRatio = Mathf.Clamp01((float)currentHp / maxHp);

        spotLight2D.pointLightOuterRadius = Mathf.Lerp(minRadius, maxRadius, hpRatio);

        spotLight2D.pointLightInnerRadius = spotLight2D.pointLightOuterRadius * 0.7f;
        #endregion

        #region ����
        if (Input.GetKeyDown(KeyCode.Z))
        {
            animator.SetTrigger("Attack");
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            animator.SetTrigger("Thrust");
        }
        #endregion
    }
    void FixedUpdate()
    {
        rb.velocity = moveInput * moveSpeed;
    }

    void Flip()
    {
        facingRight = !facingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0);
    }

    void PlayerStatReload()
    {
        maxHp = playerData.maxHp;
        moveSpeed = playerData.moveSpeed;
        damage = playerData.damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("NextStage"))
        {
            stageManager.NextStage();   
        }
    }
}
