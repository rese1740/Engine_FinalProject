using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room : MonoBehaviour
{
    public Tilemap wallTilemap;  // ���� �ִ� Ÿ�ϸ�
    public Tilemap tilemap;
    public Transform centerPoint; // �߾� ���� ������Ʈ

    public Vector3Int GetCenterCell()
    {
        return tilemap.WorldToCell(centerPoint.position);
    }
}

