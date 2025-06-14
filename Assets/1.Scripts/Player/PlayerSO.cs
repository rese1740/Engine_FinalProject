using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerSO : ScriptableObject
{
    [Header("Info")]
    public string playerName;
    public float currerntHp = 0f;
    public float maxHp = 0f; //플레이어 MaxHP
    public float moveSpeed = 5f; // 플레이어 이동속도
    public float damage = 5f; // 플레이어 데미지
    public float luck = 0f;  //게임 시작시 골드 지급량
    public float crit = 0.5f; // 크리티컬 확률
    public float critDamage = 1.5f; //크리티컬 데미지
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

}
