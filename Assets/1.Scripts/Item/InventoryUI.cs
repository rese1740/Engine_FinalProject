using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;

    public InventorySlot[] passiveSlots;
    public InventorySlot activeSlot;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            TryUseSkillFromActiveSlot();
        }
    }

    public void AddItemToSlot(ItemData item)
    {
        if (item.ArtifactType == ArtifactType.Passive)
        {
            foreach (var slot in passiveSlots)
            {
                if (!slot.HasItem())
                {
                    slot.SetItem(item);
                    return;
                }
            }
        }
        else
        {
            if (activeSlot != null && !activeSlot.HasItem())
            {
                activeSlot.SetItem(item);
            }
        }
    }

    public void TryUseSkillFromActiveSlot()
    {
        if (activeSlot != null && activeSlot.HasItem())
        {
            GameObject player = GameObject.FindWithTag("Player");
            var skillController = player.GetComponent<PlayerSkillController>();
            if (skillController != null)
            {
                skillController.UseSkill(activeSlot.GetItem());
            }
        }
    }

    public bool HasActiveItem()
    {
        return activeSlot != null && activeSlot.HasItem();
    }
}
