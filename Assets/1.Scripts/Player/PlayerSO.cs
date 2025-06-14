using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerSO : ScriptableObject
{
    [Header("Info")]
    public string playerName;
    public float currerntHp = 0f;
    public float maxHp = 0f; //�÷��̾� MaxHP
    public float moveSpeed = 5f; // �÷��̾� �̵��ӵ�
    public float damage = 5f; // �÷��̾� ������
    public float luck = 0f;  //���� ���۽� ��� ���޷�
    public float crit = 0.5f; // ũ��Ƽ�� Ȯ��
    public float critDamage = 1.5f; //ũ��Ƽ�� ������
    public int statPoint = 0;

    [Header("�κ��丮")]
    public ItemData[] passiveItems = new ItemData[2]; 
    public ItemData activeItem;

    [Header("��ȭ")]
    public int critGageIndex = 0;
    public int critDamageGageIndex = 0;
    public int strGageIndex = 0;
    public int maxHpGageIndex = 0;
    public int luckGageIndex = 0;

}
