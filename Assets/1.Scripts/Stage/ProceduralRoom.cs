using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProceduralRoom : MonoBehaviour
{
    public RoomType roomType;
    public Vector2Int gridPosition;
    public RoomTemplate template;
    public List<Vector2Int> activeDoors = new List<Vector2Int>();

    [Header("Components")]
    public Tilemap floorTilemap;
    public Tilemap wallTilemap;
    public Tilemap doorTilemap;
    public Transform centerPoint;

    public bool hasEntered = false;

    [Header("몬스터")]
    public int enemyCount = 0;  
    public GameObject[] monsterPrefabs;
    public Transform[] spawnPoints;

    private void Start()
    {
        OpenDoors();
    }

    private void Update()
    {
        if(enemyCount <= 0)
        {
            OpenDoors();
        }
    }

    public void OnPlayerEnter()
    {
        if (hasEntered) return;
        hasEntered = true;

        CloseDoors();
        SpawnMonsters();
    }

    void CloseDoors()
    {
        doorTilemap.gameObject.SetActive(true);
    }

    void OpenDoors()
    {
        doorTilemap.gameObject.SetActive(false);
    }

    void SpawnMonsters()
    {
        string[] monsterNames = { "Goblin", "Mushroom" };
        List<Vector3> validSpawnPositions = new List<Vector3>();

        BoundsInt bounds = floorTilemap.cellBounds;

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int tilePos = new Vector3Int(x, y, 0);
                if (floorTilemap.HasTile(tilePos))
                {
                    Vector3 worldPos = floorTilemap.CellToWorld(tilePos) + new Vector3(0.5f, 0.5f, 0); // 중심 보정
                    validSpawnPositions.Add(worldPos);
                }
            }
        }

        int spawnCount = Mathf.Min(3, validSpawnPositions.Count); // 최대 3마리 정도 스폰 (원하면 조정 가능)
        for (int i = 0; i < spawnCount; i++)
        {
            string randomName = monsterNames[Random.Range(0, monsterNames.Length)];
            GameObject prefab = Resources.Load<GameObject>($"Prefabs/Enemy/{randomName}");

            if (prefab != null)
            {
                int index = Random.Range(0, validSpawnPositions.Count);
                Vector3 spawnPos = validSpawnPositions[index];
                validSpawnPositions.RemoveAt(index); 

                Instantiate(prefab, spawnPos, Quaternion.identity,gameObject.transform);
                enemyCount++;
            }
            else
            {
                Debug.LogWarning($"몬스터 프리팹 로드 실패: {randomName}");
            }
        }
    }

}