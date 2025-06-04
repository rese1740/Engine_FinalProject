using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room : MonoBehaviour
{
    public Tilemap wallTilemap;  // 벽이 있는 타일맵
    public Tilemap tilemap;
    public Transform centerPoint; // 중앙 기준 오브젝트

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

