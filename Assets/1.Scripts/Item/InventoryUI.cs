using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;
    public PlayerSO playerData;
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
    private void Start()
    {
        for (int i = 0; i < passiveSlots.Length; i++)
        {
            if (playerData.passiveItems[i] != null)
            {
                passiveSlots[i].SetItem(playerData.passiveItems[i]);
            }
        }

        if (playerData.activeItem != null)
        {
            activeSlot.SetItem(playerData.activeItem);
            activeIcon.sprite = playerData.activeItem.icon;
            IconEnabled = true;
        }
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
                Debug.Log("아이템이 없습니다");
            }
        }


        activeIcon.enabled = IconEnabled;
    }

    public void AddItemToSlot(ItemData item)
    {
        if (item.ArtifactType == ArtifactType.Passive)
        {
            for (int i = 0; i < passiveSlots.Length; i++)
            {
                if (!passiveSlots[i].HasItem())
                {
                    passiveSlots[i].SetItem(item);
                    playerData.passiveItems[i] = item;
                    return;
                }
            }
        }
        else
        {
            if (activeSlot != null && !activeSlot.HasItem())
            {
                activeSlot.SetItem(item);
                playerData.activeItem = item;
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
