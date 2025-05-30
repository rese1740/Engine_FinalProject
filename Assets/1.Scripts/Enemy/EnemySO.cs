using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemySO : ScriptableObject
{
    public string enemyName;
    public float maxHp = 10f;
    public float moveSpeed = 2f;
    public float damage = 3f;
    public int goldReward = 10;
}
