using UnityEngine;

public class PlayerHitBox : MonoBehaviour
{
    public PlayerSO playerData;
    public float damage = 5f;
    public float duration = 0.1f;

    void Start()
    {
        Destroy(gameObject, duration);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyStatus enemy = other.GetComponent<EnemyStatus>();
            if (enemy != null)
            {
                float finalDamage = damage;
                if (playerData.crit <= Random.Range(0, 1))
                {
                    finalDamage *= playerData.critDamage;
                }
                Debug.Log(finalDamage);
                enemy.TakeDamage(finalDamage);
            }
        }
        else if (other.CompareTag("Boss"))
        {
            BossController enemy = other.GetComponent<BossController>();
            if (enemy != null)
            {
                float finalDamage1 = damage;
                if (playerData.crit <= Random.Range(0, 1))
                {
                    finalDamage1 *= playerData.critDamage;
                }
                enemy.TakeDamage(finalDamage1);
            }
        }
        else if (other.CompareTag("Boss2"))
        {
            MagicBoss enemy = other.GetComponent<MagicBoss>();
            if (enemy != null)
            {
                Debug.Log("Boss2 Hit");
                enemy.TakeDamage();
            }
        }
    }
}
