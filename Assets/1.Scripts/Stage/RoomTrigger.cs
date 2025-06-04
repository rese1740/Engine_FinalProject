using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    public Room parentRoom; // 수동 또는 자동 연결

    private bool hasEntered = false;

    private void Start()
    {
        if (parentRoom == null)
            parentRoom = GetComponentInParent<Room>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasEntered) return;

        if (other.CompareTag("Player"))
        {
            hasEntered = true;
            Debug.Log("플레이어가 방에 들어왔습니다: " + parentRoom.name);

            GameManager.Instance.SetCurrentRoom(parentRoom);
            parentRoom.StartRoomLogic();
        }
    }
}
