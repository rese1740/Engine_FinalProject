using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Room currentRoom;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SetCurrentRoom(Room room)
    {
        currentRoom = room;
        Debug.Log("현재 방 설정됨: " + room.name);
    }
}
