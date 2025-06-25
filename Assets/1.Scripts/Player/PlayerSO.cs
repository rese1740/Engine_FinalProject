using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerSO : ScriptableObject
{
    public static PlayerSO Instance;
    [Header("Info")]
    public string ID; // 플레이어 ID
    public string playerName;
    public float currentHp = 0f;
    public float maxHp = 0f; //플레이어 MaxHP
    public float moveSpeed = 5f; // 플레이어 이동속도
    public float damage = 5f; // 플레이어 데미지
    public float crit = 0.5f; // 크리티컬 확률
    public float critDamage = 1.5f; //크리티컬 데미지
    public int gold = 0;
    public int statPoint = 0;

    [Header("인벤토리")]
    public ItemData[] passiveItems = new ItemData[2]; 
    public ItemData activeItem;

    [Header("강화")]
    public int critGageIndex = 0;
    public int critDamageGageIndex = 0;
    public int strGageIndex = 0;
    public int maxHpGageIndex = 0;
    public int luckGageIndex = 0;

    public void Init()
    {
        Instance = this;
    }
}
