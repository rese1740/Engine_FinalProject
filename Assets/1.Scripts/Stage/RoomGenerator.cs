﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomData
{
    public Vector2Int position;
    public GameObject roomGO;
    public bool isStartRoom;
}

public class RoomGenerator : MonoBehaviour
{
    public GameObject roomPrefab;
    public int maxRooms = 10;
    public Vector2Int roomTileSize = new Vector2Int(17, 17);
    public Vector2 tileSize = new Vector2(1f, 1f);
    public int roomSpacing = 1;

    private List<RoomData> rooms = new List<RoomData>();
    private HashSet<Vector2Int> takenPositions = new HashSet<Vector2Int>();
    private Vector2Int[] directions = new Vector2Int[]
    {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right
    };

    void Start()
    {
        GenerateRooms();
    }

    void GenerateRooms()
    {
        rooms.Clear();
        takenPositions.Clear();

        RoomData startRoom = new RoomData { position = Vector2Int.zero, isStartRoom = true };
        GameObject startGO = SpawnRoom(startRoom);
        startRoom.roomGO = startGO;  
        rooms.Add(startRoom);
        takenPositions.Add(startRoom.position);

        for (int i = 1; i < maxRooms; i++)
        {
            Vector2Int newPos = GetNewPosition(out RoomData connectedFrom, out Vector2Int dirFrom);
            if (newPos == Vector2Int.zero)
            {
                Debug.LogWarning("새 방 위치 찾기 실패");
                break;
            }

            RoomData newRoom = new RoomData { position = newPos };
            GameObject newRoomGO = SpawnRoom(newRoom);
            newRoom.roomGO = newRoomGO;
            rooms.Add(newRoom);
            takenPositions.Add(newPos);
           
            List<Vector2Int> neighbors = GetNeighborDirections(newRoom.position);
            foreach (Vector2Int dir in neighbors)
            {
                Vector2Int neighborPos = newRoom.position + dir * roomSpacing;
                RoomData neighborRoom = rooms.Find(r => r.position == neighborPos);

                if (neighborRoom == null || neighborRoom.roomGO == null) continue;

                GameObject neighborGO = neighborRoom.roomGO;
                OpenWall(newRoomGO, dir);
                OpenWall(neighborGO, -dir);
               
            }
        }
    }

    Vector2Int GetNewPosition(out RoomData fromRoom, out Vector2Int fromDirection)
    {
        for (int attempt = 0; attempt < 100; attempt++)
        {
            RoomData baseRoom = rooms[Random.Range(0, rooms.Count)];
            Vector2Int dir = directions[Random.Range(0, directions.Length)];
            Vector2Int newPos = baseRoom.position + dir * roomSpacing;

            if (!takenPositions.Contains(newPos))
            {
                fromRoom = baseRoom;
                fromDirection = dir;
                return newPos;
            }
        }

        fromRoom = null;
        fromDirection = Vector2Int.zero;
        return Vector2Int.zero;
    }


    GameObject SpawnRoom(RoomData room)
    {
        Vector3 worldPos = new Vector3(
            room.position.x * roomTileSize.x * tileSize.x,
            room.position.y * roomTileSize.y * tileSize.y,
            0f);

        GameObject go = Instantiate(roomPrefab, worldPos, Quaternion.identity, transform);
        go.name = "Room_" + room.position;
        room.roomGO = go;
        return go; 
    }


    List<Vector2Int> GetNeighborDirections(Vector2Int currentPos)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        foreach (Vector2Int dir in directions)
        {
            Vector2Int checkPos = currentPos + dir * roomSpacing;
            if (takenPositions.Contains(checkPos))
            {
                neighbors.Add(dir);
            }
        }

        return neighbors;
    }


    void OpenWall(GameObject roomGO, Vector2Int direction)
    {
        Tilemap tilemap = roomGO.GetComponentInChildren<Tilemap>();
        if (tilemap == null) return;

        Vector3Int origin = tilemap.origin;
        int halfX = roomTileSize.x / 2;
        int halfY = roomTileSize.y / 2;

        List<Vector3Int> tilesToClear = new();

        if (direction == Vector2Int.right)
        {
            for (int y = -2; y <= 2; y++)
                tilesToClear.Add(new Vector3Int(origin.x + halfX, origin.y + y, 0));
        }
        else if (direction == Vector2Int.left)
        {
            for (int y = -2; y <= 2; y++)
                tilesToClear.Add(new Vector3Int(origin.x - halfX, origin.y + y, 0));
        }
        else if (direction == Vector2Int.up)
        {
            for (int x = -2; x <= 2; x++)
                tilesToClear.Add(new Vector3Int(origin.x + x, origin.y + halfY, 0));
        }
        else if (direction == Vector2Int.down)
        {
            for (int x = -2; x <= 2; x++)
                tilesToClear.Add(new Vector3Int(origin.x + x, origin.y - halfY, 0));
        }

        foreach (var pos in tilesToClear)
        {
            tilemap.SetTile(pos, null); 
        }
    }


}
