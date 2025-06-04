using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerSO : ScriptableObject
{
    [Header("Info")]
    public string playerName;
    public float currerntHp = 0f;
    public float maxHp = 0f;
    public float moveSpeed = 5f;
    public float damage = 5f;
    public float gold = 0f;

    [Header("�κ��丮")]
    public int ActiveIndex;
}
