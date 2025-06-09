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
    public float luck = 0f;  //게이 초반 골드 지급량
    public float crit = 0.5f; // 크리티컬 확률
    public float critDamage = 1.5f; //크리티컬 데미지

    [Header("인벤토리")]
    public ItemData[] passiveItems = new ItemData[2]; 
    public ItemData activeItem;

}
