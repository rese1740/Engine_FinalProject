using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerStatus : MonoBehaviour
{
    [Header("PlayerStat")]
    public PlayerSO playerData;
    public float currentHp;
    public float maxHp;
    public float moveSpeed = 5f;
    public float damage = 5f;

    [Header("변수")]
    private Vector2 moveInput;
    bool facingRight = true;
    public float minRadius = 1f;   // HP 0일 때 최소 반경
    public float maxRadius = 5f;   // HP 최대일 때 최대 반경

    [Header("UI")]
    public GameObject InventoryUI;
    public TextMeshProUGUI[] StatusTmp;


    [Header("Managers")]
    public StageManager stageManager;

    [Header("Component")]
    public Light2D spotLight2D;
    private Rigidbody2D rb;
    Animator animator;

    void Start()
    {
        PlayerStatReload();
        currentHp = maxHp;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        #region 이동
        // 입력 받기
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput = moveInput.normalized;

        // 애니메이션: 걷기 상태
        animator.SetBool("isMoving", moveInput.magnitude > 0);

        // 방향 전환 처리
        if (moveInput.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveInput.x < 0 && facingRight)
        {
            Flip();
        }
        #endregion

        #region 빛
        float hpRatio = Mathf.Clamp01((float)currentHp / maxHp);

        spotLight2D.pointLightOuterRadius = Mathf.Lerp(minRadius, maxRadius, hpRatio);

        spotLight2D.pointLightInnerRadius = spotLight2D.pointLightOuterRadius * 0.7f;
        #endregion

        #region 공격
        if (Input.GetKeyDown(KeyCode.Z))
        {
            animator.SetTrigger("Attack");
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            animator.SetTrigger("Thrust");
        }
        #endregion

        #region UI

        StatusTmp[0].text = playerData.playerName;
        StatusTmp[1].text = "HP :" +  playerData.currerntHp.ToString();
        StatusTmp[2].text = "MaxHP :" + playerData.maxHp.ToString();
        StatusTmp[3].text = "Damage :" + playerData.damage.ToString();
        StatusTmp[4].text = "Agility :" + playerData.moveSpeed.ToString();
        StatusTmp[5].text = "Gold :" + playerData.gold.ToString();


        #endregion

        if (Input.GetKeyDown(KeyCode.I))
        {
            InventoryUI.gameObject.SetActive(!InventoryUI.activeSelf);
        }

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


}
