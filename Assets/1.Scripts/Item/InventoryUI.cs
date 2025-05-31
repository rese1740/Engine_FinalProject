using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;

    public InventorySlot[] passiveSlots;
    public InventorySlot activeSlot;

    [Header("Skill")]
    public float CoolTime;
    public float maxCoolTime;
    private bool IconEnabled = false;
    public Image activeIcon;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        CoolTime -= Time.deltaTime;
        activeIcon.fillAmount += Time.deltaTime / maxCoolTime;
        if (Input.GetKeyDown(KeyCode.C) && CoolTime <= 0)
        {
            if (HasActiveItem())
            {
                TryUseSkillFromActiveSlot();
                activeIcon.fillAmount = 0;
                CoolTime = maxCoolTime;
            }
            else
            {
                Debug.Log("�������� �����ϴ�");
            }
        }


        activeIcon.enabled = IconEnabled;
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
                activeIcon.sprite = item.icon;
                IconEnabled = true;
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
