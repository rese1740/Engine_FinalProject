using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room : MonoBehaviour
{
    public Tilemap wallTilemap;  // ���� �ִ� Ÿ�ϸ�
    public Tilemap tilemap;
    public Transform centerPoint; // �߾� ���� ������Ʈ

    public RoomSO roomData;

    


    void SpawnEnemy()
    {
        Instantiate(roomData.Goblin);
    }


    public Vector3Int GetCenterCell()
    {
        return tilemap.WorldToCell(centerPoint.position);
    }
}

