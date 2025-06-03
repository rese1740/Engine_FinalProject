using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapManager : MonoBehaviour
{
    public RectTransform minimapRoot;
    public GameObject cellPrefab;

    public Color defaultColor = Color.gray;
    public Color highlightColor = Color.white;

    private GameObject currentHighlight;

    // ��� �� ��: position �� �� ������Ʈ
    private Dictionary<Vector2Int, GameObject> roomIcons = new();

    public void CreateMinimap(List<RoomData> rooms)
    {
        foreach (var room in rooms)
        {
            GameObject cell = Instantiate(cellPrefab, minimapRoot);
            cell.name = $"MiniCell_{room.position}";

            // �� ��ġ ����
            Vector2 cellPos = new Vector2(room.position.x, room.position.y) * 20f;
            cell.GetComponent<RectTransform>().anchoredPosition = cellPos;

            // �ʱ� ���� (����/��Ż ����)
            Image img = cell.GetComponent<Image>();
            if (room.isStartRoom) img.color = Color.green;
            else if (room.roomGO.name.Contains("Portal")) img.color = Color.red;
            else img.color = defaultColor;

            // ��ųʸ��� ���
            roomIcons[room.position] = cell;
        }
    }

    public void HighlightRoom(Vector2Int pos)
    {
        if (currentHighlight != null)
        {
            Image prevImage = currentHighlight.GetComponent<Image>();
            if (prevImage != null && !IsSpecialRoomColor(prevImage.color))
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

    private bool IsSpecialRoomColor(Color color)
    {
        return color == Color.green || color == Color.red;
    }
}
