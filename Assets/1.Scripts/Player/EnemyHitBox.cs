using UnityEngine;

public class EnemyHitBox : MonoBehaviour
{
    public float damage = 5f;
    public float duration = 0.1f;

    void Start()
    {
        Destroy(gameObject, duration);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStatus player = other.GetComponent<PlayerStatus>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
    }
}
