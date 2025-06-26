using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerUpgradeManager : MonoBehaviour
{
    [Header("UI")]
    public Image critGageImg;
    public Image critDamageGageImg;
    public Image strGageImg;
    public Image maxHpGageImg;
    public Image luckGageImg;
    public Sprite[] Gage;
    private int GageIndex;
    public TextMeshProUGUI statPointTxt;
    public PlayerSO playerData;

    private void Start()
    {
        GageReload(critGageImg, playerData.critGageIndex);
        GageReload(critDamageGageImg, playerData.critDamageGageIndex);
        GageReload(strGageImg, playerData.strGageIndex);
        GageReload(maxHpGageImg, playerData.maxHpGageIndex);
        GageReload(luckGageImg, playerData.luckGageIndex);
    }


    private void Update()
    {
        statPointTxt.text = playerData.statPoint.ToString();
    }

    public void Crit()
    {
        if (playerData.statPoint >= 1 && GageIndex <= 9)
        {
            playerData.crit += 0.05f;
            playerData.statPoint -= 1;
            GageReload(critGageImg, playerData.critGageIndex);
            playerData.critGageIndex++;

        }
    }

    public void CritDamage()
    {
        if (playerData.statPoint >= 1 && GageIndex <= 9)
        {
            playerData.critDamage += 0.5f;
            playerData.statPoint -= 1;
        }
    }

    public void Str()
    {
        if (playerData.statPoint >= 1 && GageIndex <= 9)
        {
            playerData.damage += 1;
            playerData.statPoint -= 1;
        }
    }

    public void MaxHP()
    {
        if (playerData.statPoint >= 1 && GageIndex <= 9)
        {
            playerData.maxHp += 2f;
            playerData.statPoint -= 1;
        }
    }

    public void Luck()
    {
        if (playerData.statPoint >= 1 && GageIndex <= 9)
        {
            playerData.gold += 1;
            playerData.statPoint -= 1;
        }

    }

    public void GameStart()
    {
        int curretStage = PlayerPrefs.GetInt("curretStage", 1);

        curretStage = 0;

        PlayerPrefs.SetInt("curretStage", curretStage);
        
        playerData.currentHp = playerData.maxHp;
        SceneManager.LoadScene("Stage1");
    }

    public void Boss1()
    {
        SceneManager.LoadScene("Boss");
    }

    public void Boss2()
    {
        SceneManager.LoadScene("Boss1");
    }

    void GageReload(Image gageImg, int index)
    {
        if (index >= 0 && index < Gage.Length)
        {
            gageImg.sprite = Gage[index];
        }
    }




}
