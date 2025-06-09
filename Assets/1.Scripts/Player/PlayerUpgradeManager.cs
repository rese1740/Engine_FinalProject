using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgradeManager : MonoBehaviour
{
    public PlayerSO playerData;
    

    public void Crit()
    {
        playerData.crit += 0.05f;
    }

    public void CritDamage()
    {
        playerData.critDamage += 0.5f;
    }

    public void Str()
    {
        playerData.damage += 1;
    }

    public void MaxHP()
    {
        playerData.maxHp += 2f;
    }

    public void Luck()
    {
        playerData.luck += 1;
    }




}
