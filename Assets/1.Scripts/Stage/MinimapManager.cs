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

    private Dictionary<Vector2Int, GameObject> roomIcons = new();

   

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
            if (img != null && !IsSpecialRoomColor(img.color))
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
