using UnityEngine;

public class PlayerHitBox : MonoBehaviour
{
    public PlayerSO playerData;
    public float critValue;
    public float critDamage;
    public float damage = 5f;
    public float duration = 0.1f;

    void Start()
    {
        critValue = playerData.luck;
        critDamage = playerData.critDamage;
        Destroy(gameObject, duration);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyStatus enemy = other.GetComponent<EnemyStatus>();
            Debug.Log(damage);
            if (enemy != null)
            {
                float finalDamage = damage;
                if (critValue <= Random.Range(0, 1))
                {
                    finalDamage *= critDamage;
                }
                Debug.Log(finalDamage);
                enemy.TakeDamage(finalDamage);
            }
        }
    }
}
