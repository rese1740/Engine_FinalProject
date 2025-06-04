using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room : MonoBehaviour
{
    public Tilemap wallTilemap;  // ���� �ִ� Ÿ�ϸ�
    public Tilemap tilemap;
    public Transform centerPoint; // �߾� ���� ������Ʈ

    public RoomSO roomData;

    IEnumerator SpawnEnemy()
    {
        Instantiate(roomData.Goblin);

        yield return new WaitForSeconds(1f);
    }

    public void StartRoomLogic()
    {
        StartCoroutine(SpawnEnemy());

    }

    public Vector3Int GetCenterCell()
    {
        return tilemap.WorldToCell(centerPoint.position);
    }
}

