using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DataBaseManager : ScriptableObject
{
    public static DataBaseManager Instance;

    public int currenStage = 0; 




    public void Init()
    {
       Instance = this;
    }
}
