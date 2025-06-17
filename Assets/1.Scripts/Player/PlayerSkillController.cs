using UnityEngine;

public class PlayerSkillController : MonoBehaviour
{
    [Header("대쉬")]
    public float dashCoolTime;
    public float dashSpeed = 20f;
    public float dashDuration = 0.3f;
    private float dashTime;
    private Vector2 moveDir;
    private bool isDashing = false;
    public GhostManager ghost;

    [Header("힐")]
    public float healCoolTime;
    public float healValue;

    [Header("힘")]
    public float strCoolTime;
    public float strTime;
    public float strValue;

    [Header("무적")]
    public float invisibleCoolTime;
    public float invisibleTime;

    [Header("파이어볼")]
    public GameObject fireballPrefab; // 파이어볼 프리팹
    public Transform firePoint;       // 발사 위치
    public float fireballSpeed = 10f; // 속도

    [Header("Component")]
    Rigidbody2D rb;
    Animator animator;
    PlayerStatus status;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        status = GetComponent<PlayerStatus>();
    }



    void FixedUpdate()
    {
        if (isDashing)
        {
            rb.velocity = moveDir * dashSpeed;

            dashTime -= Time.fixedDeltaTime;
            if (dashTime <= 0)
            {
                StopDash();
            }
        }
    }

    public void UseSkill(ItemData item)
    {
        if (item == null) return;

        switch (item.SkillType)
        {
            case SkillType.Fireball:
                CastFireball();
                break;
            case SkillType.Heal:
                HealPlayer();
                break;
            case SkillType.Dash:
                DashForward();
                break;
            case SkillType.Invincible:
                StartInvisible();
                break;
            case SkillType.Strength:
                StartStr();
                break;
        }
    }

    void CastFireball()
    {
        if (fireballPrefab != null && firePoint != null)
        {
            GameObject fireball = Instantiate(fireballPrefab, firePoint.position, firePoint.rotation);

            Rigidbody rb = fireball.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = firePoint.forward * fireballSpeed;
            }
        }
        else
        {
            Debug.LogWarning("Fireball prefab or firePoint is not assigned.");
        }
    }

    #region 무적
    void StartInvisible()
    {
        PlayerStatus.Instance.isInvisible = true;
        Invoke("EndInvisible", invisibleTime);
    }

    void EndInvisible()
    {
        InventoryUI.Instance.maxCoolTime = invisibleCoolTime;
        PlayerStatus.Instance.isInvisible = false;
    }
    #endregion

    #region 힘
    void StartStr()
    {
        PlayerSO.Instance.damage += strValue;
        Invoke("EndStr", strTime);
    }

    void EndStr()
    {
        InventoryUI.Instance.maxCoolTime = strCoolTime;
        PlayerSO.Instance.damage -= strValue;
    }
    #endregion

    #region 힐
    void HealPlayer()
    {
        InventoryUI.Instance.maxCoolTime = healCoolTime;
        Debug.Log("플레이어 체력 회복!");
        status.playerData.currentHp += healValue;
        animator.SetTrigger("Heal");
    }
    #endregion

    #region 대쉬
    void DashForward()
    {
        InventoryUI.Instance.maxCoolTime = dashCoolTime;
        StartDash();
    }

    void StartDash()
    {
        moveDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        if (moveDir == Vector2.zero)
            moveDir = Vector2.right;

        isDashing = true;
        dashTime = dashDuration;
        ghost.isGhosting = true;
    }

    void StopDash()
    {
        isDashing = false;
        rb.velocity = Vector2.zero;
        ghost.isGhosting = false;
    }
    #endregion


}
