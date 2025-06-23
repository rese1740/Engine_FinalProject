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
        SceneManager.LoadScene("Stage1");
    }

    void GageReload(Image gageImg, int index)
    {
        if (index >= 0 && index < Gage.Length)
        {
            gageImg.sprite = Gage[index];
        }
    }




}
