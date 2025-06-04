using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    public Room parentRoom; // ���� �Ǵ� �ڵ� ����

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
            Debug.Log("�÷��̾ �濡 ���Խ��ϴ�: " + parentRoom.name);

            GameManager.Instance.SetCurrentRoom(parentRoom);
            parentRoom.StartRoomLogic();
        }
    }
}
