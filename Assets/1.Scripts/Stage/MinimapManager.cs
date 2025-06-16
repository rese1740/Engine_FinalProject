using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct RoomIconPrefab
{
    public RoomType type;
    public GameObject prefab;
}

public class MinimapManager : MonoBehaviour
{
    public static MinimapManager Instance;
    public RectTransform minimapRoot;
    public GameObject cellPrefab;

    public Color defaultColor = Color.gray;
    public Color highlightColor = Color.white;

    private GameObject currentHighlight;

    // ¸ðµç ¹æ ¼¿: position ¡æ ¼¿ ¿ÀºêÁ§Æ®
    private Dictionary<Vector2Int, GameObject> roomIcons = new();

    public List<RoomIconPrefab> prefabList;
    private Dictionary<RoomType, GameObject> prefabDict;
    private void Awake()
    {
        Instance = this;
        prefabDict = new();
        foreach (var entry in prefabList)
            prefabDict[entry.type] = entry.prefab;
    }

    public void CreateMinimap(List<RoomData> rooms)
    {
        foreach (var room in rooms)
        {
            var prefab = GetRoomPrefab(room);
            GameObject cell = Instantiate(prefab, minimapRoot);
            cell.name = $"MiniCell_{room.position}";

            Vector2 cellPos = new Vector2(room.position.x, room.position.y) * 40f;
            cell.GetComponent<RectTransform>().anchoredPosition = cellPos;

            roomIcons[room.position] = cell;
        }
    }

    private GameObject GetRoomPrefab(RoomData room)
    {
        if (room.isStartRoom && prefabDict.TryGetValue(RoomType.Start, out var startPrefab))
            return startPrefab;


        if (prefabDict.TryGetValue(room.roomType, out var prefab))
            return prefab;

        // fallback
        return prefabDict[RoomType.Normal];
    }

    public void HighlightRoom(Vector2Int pos)
    {
        if (currentHighlight != null)
        {
            Image prevImage = currentHighlight.GetComponent<Image>();
            if (prevImage != null)
                prevImage.color = defaultColor;
        }

        if (roomIcons.TryGetValue(pos, out var icon))
        {
            Image img = icon.GetComponent<Image>();
            if (img != null)
            {
                img.color = highlightColor;
                currentHighlight = icon;
            }
        }
    }

  
}