using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerUpgradeManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI statPointTxt;
    public PlayerSO playerData;

    private void Update()
    {
        statPointTxt.text = playerData.statPoint.ToString();
    }

    public void Crit()
    {
        if(playerData.statPoint >= 1)
        {
            playerData.crit += 0.05f;
            playerData.statPoint -= 1;
        }
    }

    public void CritDamage()
    {
        if (playerData.statPoint >= 1)
        {
            playerData.critDamage += 0.5f;
            playerData.statPoint -= 1;
        }
    }

    public void Str()
    {
        if (playerData.statPoint >= 1)
        {
            playerData.damage += 1;
            playerData.statPoint -= 1;
        }
    }

    public void MaxHP()
    {
        if (playerData.statPoint >= 1)
        {
            playerData.maxHp += 2f;
            playerData.statPoint -= 1;
        }
    }

    public void Luck()
    {
        if (playerData.statPoint >= 1)
        {
            playerData.luck += 1;
            playerData.statPoint -= 1;
        }
      
    }

    public void GameStart()
    {
        SceneManager.LoadScene("Stage1");
    }



}
