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

[System.Serializable]
public class RoomTemplate
{
    public Vector2Int size = new Vector2Int(17, 17);
    public List<Vector2Int> doorPositions = new List<Vector2Int>();
    public List<Vector2Int> obstaclePositions = new List<Vector2Int>();
}

public class ProceduralRoom : MonoBehaviour
{
    public Vector2Int gridPosition;
    public RoomTemplate template;
    public List<Vector2Int> activeDoors = new List<Vector2Int>();

    [Header("Components")]
    public Tilemap floorTilemap;
    public Tilemap wallTilemap;
    public Transform centerPoint;

    private void Start()
    {
       
    }
}

public class ProceduralRoomGenerator : MonoBehaviour
{
    [Header("Tile Settings")]
    public TileSet tileSet;

    [Header("Room Templates")]
    public List<RoomTemplate> roomTemplates;

    [Header("Generation Settings")]
    public int maxRooms = 8;

    [Header("Grid Settings")]
    public float roomSpacing = 17f;
    public Vector2Int gridSize = new Vector2Int(5, 5);

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
        floorObj.AddComponent<TilemapRenderer>().sortingOrder = 0;

        GameObject wallObj = new GameObject("Wall");
        wallObj.transform.parent = roomObj.transform;
        wallObj.transform.localPosition = Vector3.zero;

        Tilemap wallTilemap = wallObj.AddComponent<Tilemap>();
        TilemapCollider2D WallTilemap = wallTilemap.gameObject.AddComponent<TilemapCollider2D>();
        wallObj.AddComponent<TilemapRenderer>().sortingOrder = 1;

        GameObject centerObj = new GameObject("Center");
        centerObj.transform.parent = roomObj.transform;
        centerObj.transform.localPosition = new Vector3(8.5f, 8.5f, 0f);

        ProceduralRoom roomComponent = roomObj.AddComponent<ProceduralRoom>();
        roomComponent.floorTilemap = floorTilemap;
        roomComponent.wallTilemap = wallTilemap;
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
        Vector3Int pos = new Vector3Int(doorPos.x, doorPos.y, 0);
        room.wallTilemap.SetTile(pos, null);
        room.floorTilemap.SetTile(pos, tileSet.floorTile);
        room.activeDoors.Add(doorPos);
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

        Tilemap tilemap = bridge.AddComponent<Tilemap>();
        bridge.AddComponent<TilemapRenderer>().sortingOrder = 0;

        PlaceBridgeTiles(tilemap, start, end, direction);
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

    void PlaceBridgeTiles(Tilemap tilemap, Vector3 start, Vector3 end, Vector2Int dir)
    {
        Vector3Int startCell = tilemap.WorldToCell(start);
        Vector3Int endCell = tilemap.WorldToCell(end);

        if (dir == Vector2Int.right || dir == Vector2Int.left)
        {
            int xStart = Mathf.Min(startCell.x, endCell.x);
            int xEnd = Mathf.Max(startCell.x, endCell.x);
            int y = startCell.y;
            for (int x = xStart; x <= xEnd; x++)
                tilemap.SetTile(new Vector3Int(x, y, 0), tileSet.floorTile);
        }
        else if (dir == Vector2Int.up || dir == Vector2Int.down)
        {
            int yStart = Mathf.Min(startCell.y, endCell.y);
            int yEnd = Mathf.Max(startCell.y, endCell.y);
            int x = startCell.x;
            for (int y = yStart; y <= yEnd; y++)
                tilemap.SetTile(new Vector3Int(x, y, 0), tileSet.floorTile);
        }
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
