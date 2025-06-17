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

    [Header("대쉬")]
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
        }
    }

    void CastFireball()
    {

    }

    void HealPlayer()
    {
        InventoryUI.Instance.maxCoolTime = healCoolTime;
        Debug.Log("플레이어 체력 회복!");
        status.playerData.currentHp += healValue;
        animator.SetTrigger("Heal");
    }

    void DashForward()
    {
        InventoryUI.Instance.maxCoolTime = dashCoolTime;
        StartDash();
    }

    #region 대쉬
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
