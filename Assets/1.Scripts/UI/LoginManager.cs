using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
[System.Serializable]
public class SaveData
{
    [Header("Info")]
    public string ID;
    public string playerName;
    public float currentHp = 15f;
    public float maxHp = 15f; 
    public float moveSpeed = 5f; 
    public float damage = 5f; 
    public float crit = 0.5f; 
    public float critDamage = 1.5f; 
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
            // 새로운 플레이어 생성
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
                gold = 5,
                statPoint = 0,

                critGageIndex = 0,  
                critDamageGageIndex = 0,
                strGageIndex = 0,
                maxHpGageIndex = 0,
                luckGageIndex = 0,
            };

            SaveByID(data);
            Debug.Log($"New player '{inputID}' created and saved.");
        }
        else
        {
            Debug.Log($"Loaded existing player: {data.playerName}");
        }

        ApplyToPlayerData(data);
        SceneManager.LoadScene("Tutorial");
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

    private void ApplyToPlayerData(SaveData data)
    {
        playerData.ID = data.ID;
        playerData.playerName = data.playerName;
        playerData.currentHp = data.currentHp;
        playerData.maxHp = data.maxHp;
        playerData.moveSpeed = data.moveSpeed;
        playerData.damage = data.damage;
        playerData.crit = data.crit;
        playerData.critDamage = data.critDamage;
        playerData.gold = data.gold;
        playerData.statPoint = data.statPoint;

        playerData.passiveItems = data.passiveItems;
        playerData.activeItem = data.activeItem;

        playerData.critGageIndex = data.critGageIndex;
        playerData.critDamageGageIndex = data.critDamageGageIndex;
        playerData.strGageIndex = data.strGageIndex;
        playerData.maxHpGageIndex = data.maxHpGageIndex;
        playerData.luckGageIndex = data.luckGageIndex;
    }
}
