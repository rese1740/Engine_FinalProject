using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
[System.Serializable]
public class SaveData
{
    public string ID;
    public string playerName;
    public float currentHp = 0f;
    public float maxHp = 0f; //�÷��̾� MaxHP
    public float moveSpeed = 5f; // �÷��̾� �̵��ӵ�
    public float damage = 5f; // �÷��̾� ������
    public float luck = 0f;  //���� ���۽� ��� ���޷�
    public float crit = 0.5f; // ũ��Ƽ�� Ȯ��
    public float critDamage = 1.5f; //ũ��Ƽ�� ������
    public int gold = 0;
    public int statPoint = 0;
}

public class LoginManager : MonoBehaviour
{
    public void SaveByID(SaveData data)
    {
        string json = JsonUtility.ToJson(data);
        string path = Application.persistentDataPath + $"/save_{data.ID}.json";
        File.WriteAllText(path, json);
    }

    public SaveData LoadByID(string id)
    {
        string path = Application.persistentDataPath + $"/save_{id}.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            return data;
        }
        else
        {
            Debug.LogWarning($"Save file with ID '{id}' not found.");
            return null;
        }
    }
}
