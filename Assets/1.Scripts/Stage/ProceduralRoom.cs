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

    public GameObject[] monsterPrefabs;
    public Transform[] spawnPoints;

    private void Start()
    {
        roomType = template.roomType;
        OpenDoors();
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
        foreach (var point in spawnPoints)
        {
            Instantiate(monsterPrefabs[Random.Range(0, monsterPrefabs.Length)], point.position, Quaternion.identity);
        }
    }

}