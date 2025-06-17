using System;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerStatus : MonoBehaviour
{
    [Header("PlayerStat")]
    public static PlayerStatus Instance;
    public PlayerSO playerData;
    public float moveSpeed = 5f;
    public float damage = 5f;

    [Header("변수")]
    private Vector2 moveInput;
    bool facingRight = true;
    public float minRadius = 1f;   // HP 0일 때 최소 반경
    public float maxRadius = 5f;   // HP 최대일 때 최대 반경

    [Header("Combat")]
    public GameObject smashHitboxPrefab;
    public GameObject thrustHitboxPrefab;
    public Transform attackPoint;


    public float attackDamage = 5f;
    public float thrustDamage = 8f;


    [Header("UI")]
    public GameObject InventoryUI;
    public TextMeshProUGUI[] StatusTmp;

    [Header("Managers")]
    public ProceduralRoomGenerator stageManager;

    [Header("Component")]
    public Light2D spotLight2D;
    private Rigidbody2D rb;
    Animator animator;

    void Start()
    {
        Instance = this;
        PlayerStatReload();
        playerData.currentHp = playerData.maxHp;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        #region 이동
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput = moveInput.normalized;

        animator.SetBool("isMoving", moveInput.magnitude > 0);

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
        float hpRatio = Mathf.Clamp01((float)playerData.currentHp / playerData.maxHp);

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
        StatusTmp[1].text = "HP :" +  playerData.currentHp.ToString();
        StatusTmp[2].text = "MaxHP :" + playerData.maxHp.ToString();
        StatusTmp[3].text = "Damage :" + playerData.damage.ToString();
        StatusTmp[4].text = "Agility :" + playerData.moveSpeed.ToString();
        StatusTmp[5].text = "Gold :" + playerData.gold.ToString();


        #endregion

        if (Input.GetKeyDown(KeyCode.V))
        {
            InventoryUI.gameObject.SetActive(!InventoryUI.activeSelf);
        }
    }
    public void PassiveApply(ItemData item)
    {
        if (item == null) return;

        switch (item.PassiveType)
        {
            case PassiveType.Str:
                damage += item.Value; 
                break;
            case PassiveType.HP:
                playerData.currentHp += item.Value;
                break;
            case PassiveType.Agility:
                moveSpeed += item.Value;
                break;
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

    public void SpawnSmashHitbox()
    {
        GameObject hitbox = Instantiate(smashHitboxPrefab, attackPoint.position, Quaternion.identity);
        hitbox.GetComponent<PlayerHitBox>().damage = attackDamage;
    }

    public void SpawnThrustHitbox()
    {
        GameObject hitbox = Instantiate(thrustHitboxPrefab, attackPoint.position, Quaternion.identity);
        hitbox.GetComponent<PlayerHitBox>().damage = thrustDamage;
    }

    public void TakeDamage(float damage)
    {
        playerData.currentHp -= damage;
        playerData.currentHp = Mathf.Max(playerData.currentHp, 0);
    }

    void PlayerStatReload()
    {
        moveSpeed = playerData.moveSpeed;
        damage = playerData.damage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Portal"))
        {
            int StageIndex = SceneManager.GetActiveScene().buildIndex;
            int NextStageIndex = StageIndex += 1;
            SceneManager.LoadScene(NextStageIndex);
        }
        else if (other.CompareTag("Coin"))
        {
            playerData.gold += 1;
            Destroy(other.gameObject);
        }
    }

}
