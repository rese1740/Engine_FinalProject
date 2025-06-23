using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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
    public TMP_InputField idInputField;

    public PlayerSO playerData;

    public void OnLoginButtonClick()
    {
        string inputID = idInputField.text;

        if (string.IsNullOrEmpty(inputID))
        {
            Debug.LogWarning("ID is empty!");
            return;
        }

        SaveData data = LoadByID(inputID);

        if (data == null)
        {
            // ���ο� �÷��̾� ����
            data = new SaveData
            {
                ID = inputID,
                playerName = inputID,
                maxHp = 15f,
                currentHp = 15f,
                moveSpeed = 5f,
                damage = 5f,
                crit = 0.3f,
                critDamage = 1.5f,
                gold = 0,
                statPoint = 0
            };

            SaveByID(data);
            Debug.Log($"New player '{inputID}' created and saved.");
        }
        else
        {
            Debug.Log($"Loaded existing player: {data.playerName}");
        }

        // ���� PlayerStatus � ������ ����
        // PlayerStatus.Instance.LoadFromSaveData(data);
    }

    public void SaveByID(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true); 
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
            return null;
        }
    }
}
