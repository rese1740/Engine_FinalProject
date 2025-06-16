using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


[System.Serializable]
public class TileSet
{
    [Header("Wall Tiles")]
    public TileBase cornerTopLeft;
    public TileBase cornerTopRight;
    public TileBase cornerBottomLeft;
    public TileBase cornerBottomRight;
    public TileBase wallTop;
    public TileBase wallBottom;
    public TileBase wallLeft;
    public TileBase wallRight;

    [Header("Floor Tiles")]
    public TileBase floorTile;
    public TileBase[] floorVariations;
}

public struct RoomData
{
    public Vector2Int position;
    public GameObject roomGO;
    public bool isStartRoom;
    public RoomType roomType; // 추가
}


public enum RoomType
{
    Normal,
    Start,
    Portal,
    Shop,
    Hidden
}

[System.Serializable]
public class RoomTemplate
{
    public RoomType roomType = RoomType.Normal;
    public Vector2Int size = new Vector2Int(17, 17);
    public List<Vector2Int> doorPositions = new List<Vector2Int>();
    public List<Vector2Int> obstaclePositions = new List<Vector2Int>();
}



public class ProceduralRoomGenerator : MonoBehaviour
{
    [Header("Tile Settings")]
    public TileSet tileSet;

    [Header("Room Templates")]
    public List<RoomTemplate> roomTemplates;
    public GameObject portalPrefab;
    public GameObject shopPrefab;
    public GameObject roomTrigger;

    [Header("Generation Settings")]
    public int maxRooms = 8;

    [Header("Grid Settings")]
    public float roomSpacing = 17f;
    public Vector2Int gridSize = new Vector2Int(5, 5);

    [Header("Bridge Settings")]
    public int bridgeWidth = 3;

    [Header("Systems")]
    public MinimapManager minimapManager;
    public Transform player;

    private List<ProceduralRoom> generatedRooms = new List<ProceduralRoom>();
    private bool[,] gridOccupied;
    private Vector2Int lastPlayerGridPos;

    private Vector2Int[] directions = {
        Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
    };

    void Start()
    {
        ValidateTileSet();
        InitializeGrid();
        GenerateProceduralDungeon();
    }

    void Update()
    {
        UpdatePlayerTracking();
    }

    void ValidateTileSet()
    {
        if (tileSet.floorTile == null)
            Debug.LogError("Floor tile is missing! Please assign floor tile in TileSet.");

        if (tileSet.wallTop == null)
            Debug.LogError("Wall tiles are missing! Please assign wall tiles in TileSet.");
    }

    void InitializeGrid()
    {
        gridOccupied = new bool[gridSize.x, gridSize.y];
    }

    void GenerateProceduralDungeon()
    {
        ClearPrevious();

        for (int i = 0; i < maxRooms; i++)
        {
            RoomTemplate template = SelectRoomTemplate();
            CreateProceduralRoom(i, template);
        }

        Invoke("ConnectAllRooms", 0.2f);

        AssignSpecialRooms();
        SetupMinimap();

    }
    void AssignSpecialRooms()
    {
        if (generatedRooms.Count == 0) return;

        foreach (var room in generatedRooms)
        {
            room.template.roomType = RoomType.Normal;
        }

        HashSet<ProceduralRoom> assignedRooms = new HashSet<ProceduralRoom>();

        // 시작 방
        ProceduralRoom firstRoom = generatedRooms[0];
        if (firstRoom != null)
        {
            Debug.Log("시작방");
            firstRoom.template.roomType = RoomType.Start;
            player.transform.position = firstRoom.centerPoint.position;
            assignedRooms.Add(firstRoom);
        }

        // 포탈 방
        ProceduralRoom furthestRoom = FindFurthestRoomFromStart();
        if (furthestRoom != null && !assignedRooms.Contains(furthestRoom))
        {
            furthestRoom.template.roomType = RoomType.Portal;
            Instantiate(portalPrefab, furthestRoom.centerPoint.position, Quaternion.identity, furthestRoom.transform);
            assignedRooms.Add(furthestRoom);
        }

        // 상점 방
        if (generatedRooms.Count >= 3)
        {
            ProceduralRoom shopRoom = generatedRooms[generatedRooms.Count / 2];
            if (!assignedRooms.Contains(shopRoom))
            {
                shopRoom.template.roomType = RoomType.Shop;
                Instantiate(shopPrefab, shopRoom.centerPoint.position, Quaternion.identity, shopRoom.transform);
                assignedRooms.Add(shopRoom);
            }
        }

        // 이벤트 방 
        List<ProceduralRoom> outerRooms = generatedRooms.FindAll(r =>
        {
            if (assignedRooms.Contains(r)) return false;

            int neighbors = 0;
            foreach (var dir in directions)
            {
                if (FindRoomAtGridPosition(r.gridPosition + dir) != null)
                    neighbors++;
            }
            return neighbors <= 1;
        });

        if (outerRooms.Count > 0)
        {
            ProceduralRoom hidden = outerRooms[Random.Range(0, outerRooms.Count)];
            hidden.template.roomType = RoomType.Hidden;
            assignedRooms.Add(hidden);
        }

        
      
    }

    ProceduralRoom FindFurthestRoomFromStart()
    {
        if (generatedRooms.Count == 0)
            return null;

        Vector2Int startPos = generatedRooms[0].gridPosition;
        ProceduralRoom furthestRoom = null;
        int maxDistance = -1;

        foreach (var room in generatedRooms)
        {
            int distance = Mathf.Abs(room.gridPosition.x - startPos.x) + Mathf.Abs(room.gridPosition.y - startPos.y); // Manhattan distance

            if (distance > maxDistance)
            {
                maxDistance = distance;
                furthestRoom = room;
            }
        }

        return furthestRoom;
    }
    void ConnectAllRooms()
    {
        foreach (var room in generatedRooms)
        {
            if (room == null || room.wallTilemap == null) continue;

            foreach (var dir in directions)
            {
                Vector2Int adjacentPos = room.gridPosition + dir;
                ProceduralRoom adjacentRoom = FindRoomAtGridPosition(adjacentPos);

                if (adjacentRoom != null && adjacentRoom.wallTilemap != null)
                {
                    if (!IsConnectedTo(room, adjacentRoom, dir))
                    {
                        CreateDoorConnection(room, adjacentRoom, dir);
                    }
                }
            }
        }
    }

    bool IsConnectedTo(ProceduralRoom roomA, ProceduralRoom roomB, Vector2Int direction)
    {
        Vector2Int expectedDoorPos = CalculateDoorPosition(roomA.template.size, direction);
        return roomA.activeDoors.Contains(expectedDoorPos);
    }

    void ClearPrevious()
    {
        for (int i = generatedRooms.Count - 1; i >= 0; i--)
        {
            if (generatedRooms[i] != null && generatedRooms[i].gameObject != null)
                DestroyImmediate(generatedRooms[i].gameObject);
        }

        foreach (Transform child in transform)
        {
            if (child.name.StartsWith("Bridge"))
                DestroyImmediate(child.gameObject);
        }

        generatedRooms.Clear();
        gridOccupied = new bool[gridSize.x, gridSize.y];
    }

    RoomTemplate SelectRoomTemplate()
    {
        if (roomTemplates.Count > 0)
            return roomTemplates[Random.Range(0, roomTemplates.Count)];

        return new RoomTemplate { size = new Vector2Int(17, 17) };
    }

    void CreateProceduralRoom(int index, RoomTemplate template)
    {
        Vector2Int gridPos = FindValidGridPosition(index);
        if (gridPos == Vector2Int.one * -1) return;

        Vector3 worldPos = GridToWorldPosition(gridPos);
        GameObject roomObj = CreateRoomGameObject(worldPos, gridPos);

        ProceduralRoom room = roomObj.GetComponent<ProceduralRoom>();
        room.gridPosition = gridPos;
        room.template = template;
        room.roomType = template.roomType;

        StartCoroutine(GenerateRoomTilesDelayed(room));

        generatedRooms.Add(room);
        gridOccupied[gridPos.x, gridPos.y] = true;
    }

    GameObject CreateRoomGameObject(Vector3 worldPos, Vector2Int gridPos)
    {
        GameObject roomObj = new GameObject($"ProceduralRoom_{gridPos.x}_{gridPos.y}");
        roomObj.transform.position = worldPos;
        roomObj.transform.parent = transform;

        Grid grid = roomObj.AddComponent<Grid>();
        grid.cellSize = new Vector3(1f, 1f, 0f);

        GameObject floorObj = new GameObject("Floor");
        floorObj.transform.parent = roomObj.transform;
        floorObj.transform.localPosition = Vector3.zero;

        Tilemap floorTilemap = floorObj.AddComponent<Tilemap>();
        floorObj.AddComponent<TilemapRenderer>().sortingOrder = -2;

        GameObject wallObj = new GameObject("Wall");
        wallObj.transform.parent = roomObj.transform;
        wallObj.transform.localPosition = Vector3.zero;

        Tilemap wallTilemap = wallObj.AddComponent<Tilemap>();
        TilemapCollider2D WallTilemap = wallTilemap.gameObject.AddComponent<TilemapCollider2D>();
        wallObj.AddComponent<TilemapRenderer>().sortingOrder = -1;

        GameObject doorObj = new GameObject("Door");
        doorObj.transform.parent = roomObj.transform;
        doorObj.transform.localPosition = Vector3.zero;

        Tilemap doorTilemap = doorObj.AddComponent<Tilemap>();
        TilemapCollider2D DoorTilemap = doorTilemap.gameObject.AddComponent<TilemapCollider2D>();
        doorObj.AddComponent<TilemapRenderer>().sortingOrder = -1;

        GameObject centerObj = new GameObject("Center");
        centerObj.transform.parent = roomObj.transform;
        centerObj.transform.localPosition = new Vector3(8.5f, 8.5f, 0f);

        ProceduralRoom roomComponent = roomObj.AddComponent<ProceduralRoom>();
        roomComponent.floorTilemap = floorTilemap;
        roomComponent.wallTilemap = wallTilemap;
        roomComponent.doorTilemap = doorTilemap;
        roomComponent.centerPoint = centerObj.transform;

        return roomObj;
    }

    Vector2Int FindValidGridPosition(int index)
    {
        if (index == 0)
            return new Vector2Int(gridSize.x / 2, gridSize.y / 2);

        List<Vector2Int> candidates = new List<Vector2Int>();

        foreach (var room in generatedRooms)
        {
            foreach (var dir in directions)
            {
                Vector2Int candidate = room.gridPosition + dir;
                if (IsValidGridPosition(candidate) && !gridOccupied[candidate.x, candidate.y])
                    candidates.Add(candidate);
            }
        }

        return candidates.Count > 0 ? candidates[Random.Range(0, candidates.Count)] : Vector2Int.one * -1;
    }
    

    bool IsValidGridPosition(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < gridSize.x && pos.y >= 0 && pos.y < gridSize.y;
    }

    Vector3 GridToWorldPosition(Vector2Int gridPos)
    {
        return new Vector3(gridPos.x * roomSpacing, gridPos.y * roomSpacing, 0f);
    }

    IEnumerator GenerateRoomTilesDelayed(ProceduralRoom room)
    {
        yield return null;
        GenerateRoomTiles(room);
    }

    void GenerateRoomTiles(ProceduralRoom room)
    {
        Vector2Int size = room.template.size;
        GenerateFloor(room.floorTilemap, size);
        GenerateWalls(room.wallTilemap, size);
        GenerateWalls(room.doorTilemap, size);

        switch (room.template.roomType)
        {
            case RoomType.Portal:
                // 보스 타일 배치 or 보스 prefab 생성
                break;
            case RoomType.Shop:
                // 상점 NPC, 상점 아이템 배치
                break;
            case RoomType.Hidden:
                // 안 보이게 하거나 특정 이벤트 후 열리게
                break;
        }
    }

    void GenerateFloor(Tilemap floorTilemap, Vector2Int size)
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
                floorTilemap.SetTile(new Vector3Int(x, y, 0), tileSet.floorTile);
        }
    }

    void GenerateWalls(Tilemap wallTilemap, Vector2Int size)
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                if (x == 0 || y == 0 || x == size.x - 1 || y == size.y - 1)
                    wallTilemap.SetTile(new Vector3Int(x, y, 0), GetWallTile(x, y, size));
            }
        }
    }

    TileBase GetWallTile(int x, int y, Vector2Int size)
    {
        bool isLeft = x == 0;
        bool isRight = x == size.x - 1;
        bool isTop = y == size.y - 1;
        bool isBottom = y == 0;

        if (isLeft && isTop) return tileSet.cornerTopLeft;
        if (isRight && isTop) return tileSet.cornerTopRight;
        if (isLeft && isBottom) return tileSet.cornerBottomLeft;
        if (isRight && isBottom) return tileSet.cornerBottomRight;

        if (isTop) return tileSet.wallTop;
        if (isBottom) return tileSet.wallBottom;
        if (isLeft) return tileSet.wallLeft;
        if (isRight) return tileSet.wallRight;

        return tileSet.wallTop;
    }

    void CreateDoorConnection(ProceduralRoom roomA, ProceduralRoom roomB, Vector2Int direction)
    {
        Vector2Int doorA = CalculateDoorPosition(roomA.template.size, direction);
        Vector2Int doorB = CalculateDoorPosition(roomB.template.size, -direction);

        CreateDoor(roomA, doorA);
        CreateDoor(roomB, doorB);

        CreateConnectionTile(roomA, roomB, doorA, doorB, direction);
    }

    void CreateDoor(ProceduralRoom room, Vector2Int doorPos)
    {
        int halfWidth = bridgeWidth / 2;

        // 방향 판단 (가장자리 기준)
        bool horizontal = doorPos.y == 0 || doorPos.y == room.template.size.y - 1; // 상하 문
        bool vertical = doorPos.x == 0 || doorPos.x == room.template.size.x - 1;   // 좌우 문

        for (int offset = -halfWidth; offset <= halfWidth; offset++)
        {
            Vector2Int pos;
            if (horizontal)
                pos = new Vector2Int(doorPos.x + offset, doorPos.y);
            else if (vertical)
                pos = new Vector2Int(doorPos.x, doorPos.y + offset);
            else
                pos = doorPos; // 예외: 가운데 등 잘못된 위치일 때

            Vector3Int tilePos = new Vector3Int(pos.x, pos.y, 0);
            room.wallTilemap.SetTile(tilePos, null);
            room.floorTilemap.SetTile(tilePos, tileSet.floorTile);

            if (!room.activeDoors.Contains(pos))
                room.activeDoors.Add(pos);
        }
    }


    void CreateConnectionTile(ProceduralRoom roomA, ProceduralRoom roomB, Vector2Int doorA, Vector2Int doorB, Vector2Int direction)
    {
        if (roomSpacing <= 17f) return;

        Vector3 start = GetBridgeStartPoint(roomA, doorA, direction);
        Vector3 end = GetBridgeEndPoint(roomB, doorB, direction);

        GameObject bridge = new GameObject($"Bridge_{roomA.gridPosition}_{roomB.gridPosition}");
        bridge.transform.position = start;
        bridge.transform.parent = transform;

        Grid grid = bridge.AddComponent<Grid>();
        grid.cellSize = new Vector3(1f, 1f, 0f);

        // 바닥 타일맵
        GameObject floorObj = new GameObject("BridgeFloor");
        floorObj.transform.parent = bridge.transform;
        Tilemap floorMap = floorObj.AddComponent<Tilemap>();
        floorObj.AddComponent<TilemapRenderer>().sortingOrder = 0;

        // 벽 타일맵
        GameObject wallObj = new GameObject("BridgeWall");
        wallObj.transform.parent = bridge.transform;
        Tilemap wallMap = wallObj.AddComponent<Tilemap>();
        TilemapRenderer wallRenderer = wallObj.AddComponent<TilemapRenderer>();
        wallRenderer.sortingOrder = 1;

        // 충돌 설정
        var collider = wallObj.AddComponent<TilemapCollider2D>();
        collider.usedByComposite = true;
        wallObj.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        wallObj.AddComponent<CompositeCollider2D>().geometryType = CompositeCollider2D.GeometryType.Polygons;

        PlaceBridgeTiles(floorMap, wallMap, start, end, direction);
    }


    Vector3 GetBridgeStartPoint(ProceduralRoom room, Vector2Int doorPos, Vector2Int dir)
    {
        Vector3 basePos = room.transform.position;
        if (dir == Vector2Int.right) return basePos + new Vector3(room.template.size.x, doorPos.y, 0);
        if (dir == Vector2Int.left) return basePos + new Vector3(-1, doorPos.y, 0);
        if (dir == Vector2Int.up) return basePos + new Vector3(doorPos.x, room.template.size.y, 0);
        if (dir == Vector2Int.down) return basePos + new Vector3(doorPos.x, -1, 0);
        return basePos;
    }

    Vector3 GetBridgeEndPoint(ProceduralRoom room, Vector2Int doorPos, Vector2Int dir)
    {
        Vector3 basePos = room.transform.position;
        if (dir == Vector2Int.right) return basePos + new Vector3(0, doorPos.y, 0);
        if (dir == Vector2Int.left) return basePos + new Vector3(room.template.size.x, doorPos.y, 0);
        if (dir == Vector2Int.up) return basePos + new Vector3(doorPos.x, 0, 0);
        if (dir == Vector2Int.down) return basePos + new Vector3(doorPos.x, room.template.size.y, 0);
        return basePos;
    }

    void PlaceBridgeTiles(Tilemap floorMap, Tilemap wallMap, Vector3 start, Vector3 end, Vector2Int dir)
    {
        Vector3Int startCell = floorMap.WorldToCell(start);
        Vector3Int endCell = floorMap.WorldToCell(end);
        int halfWidth = bridgeWidth / 2;

        if (dir == Vector2Int.right || dir == Vector2Int.left)
        {
            int xStart = Mathf.Min(startCell.x, endCell.x);
            int xEnd = Mathf.Max(startCell.x, endCell.x);

            for (int x = xStart; x <= xEnd; x++)
            {
                for (int offset = -halfWidth - 1; offset <= halfWidth + 1; offset++)
                {
                    Vector3Int pos = new Vector3Int(x, startCell.y + offset, 0);

                    if (offset >= -halfWidth && offset <= halfWidth)
                        floorMap.SetTile(pos, tileSet.floorTile);
                    else
                        wallMap.SetTile(pos, tileSet.wallTop);
                }
            }
        }
        else if (dir == Vector2Int.up || dir == Vector2Int.down)
        {
            int yStart = Mathf.Min(startCell.y, endCell.y);
            int yEnd = Mathf.Max(startCell.y, endCell.y);

            for (int y = yStart; y <= yEnd; y++)
            {
                for (int offset = -halfWidth - 1; offset <= halfWidth + 1; offset++)
                {
                    Vector3Int pos = new Vector3Int(startCell.x + offset, y, 0);

                    if (offset >= -halfWidth && offset <= halfWidth)
                        floorMap.SetTile(pos, tileSet.floorTile);
                    else
                        wallMap.SetTile(pos, tileSet.wallTop);
                }
            }
        }
    }

    void SetupMinimap()
    {
        if (minimapManager == null) return;

        List<RoomData> list = new List<RoomData>();
        foreach (var room in generatedRooms)
        {
            list.Add(new RoomData
            {
                position = room.gridPosition,
                roomGO = room.gameObject,
                isStartRoom = room == generatedRooms[0], // 첫 방이면 시작방
                roomType = room.template.roomType         // 핵심 추가 부분
            });
        }

        minimapManager.CreateMinimap(list);
        
    }



    ProceduralRoom FindRoomAtGridPosition(Vector2Int pos)
    {
        return generatedRooms.Find(r => r.gridPosition == pos);
    }


    void UpdatePlayerTracking()
    {
        if (player == null) return;
        Vector2Int currentPos = GetPlayerGridPosition(player.position);
        if (currentPos != lastPlayerGridPos)
        {
            lastPlayerGridPos = currentPos;
            minimapManager?.HighlightRoom(currentPos);
        }
    }

    public Vector2Int GetPlayerGridPosition(Vector3 worldPos)
    {
        int x = Mathf.RoundToInt(worldPos.x / roomSpacing);
        int y = Mathf.RoundToInt(worldPos.y / roomSpacing);
        return new Vector2Int(x, y);
    }

    Vector2Int CalculateDoorPosition(Vector2Int roomSize, Vector2Int direction)
    {
        Vector2Int center = roomSize / 2;

        if (direction == Vector2Int.up)
            return new Vector2Int(center.x, roomSize.y - 1);
        if (direction == Vector2Int.down)
            return new Vector2Int(center.x, 0);
        if (direction == Vector2Int.left)
            return new Vector2Int(0, center.y);
        if (direction == Vector2Int.right)
            return new Vector2Int(roomSize.x - 1, center.y);

        return center;
    }
}
