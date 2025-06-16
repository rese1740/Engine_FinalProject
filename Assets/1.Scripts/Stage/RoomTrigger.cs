using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    private ProceduralRoom room;

    void Start()
    {
        room = GetComponentInParent<ProceduralRoom>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            room.OnPlayerEnter();
        }
    }
}
