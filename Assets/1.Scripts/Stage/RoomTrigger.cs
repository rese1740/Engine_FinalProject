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
            switch (room.template.roomType)
            {
                case RoomType.Normal:
                    room.OnPlayerEnter();
                    MinimapManager.Instance.HighlightRoom(room.gridPosition);
                    break;

                case RoomType.Portal:
                case RoomType.Shop:
                case RoomType.Start:
                case RoomType.Hidden:
                    MinimapManager.Instance.HighlightRoom(room.gridPosition);
                    break;

            }
        }
    }
}
