using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;

    public InventorySlot[] passiveSlots;
    public InventorySlot[] activeSlots;

    private void Start()
    {
        Instance = this;
    }

    public void AddItemToSlot(ItemData item)
    {
        if (item.ItemType != ItemType.Artifact)
        {
            Debug.Log("�̰� ��Ƽ��Ʈ �������� �ƴ�");
            return;
        }

        InventorySlot[] targetSlots = item.ArtifactType == ArtifactType.Passive ? passiveSlots : activeSlots;

        // �� ���� ã��
        foreach (var slot in targetSlots)
        {
            if (!slot.HasItem())
            {
                slot.SetItem(item);
                return;
            }
        }
    }
}
