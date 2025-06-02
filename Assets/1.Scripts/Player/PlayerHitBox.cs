using UnityEngine;

public class PlayerHitBox: MonoBehaviour
{
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
                enemy.TakeDamage(damage);
            }
        }
    }
}
