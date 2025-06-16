using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Animal
{
    public string name;
    public string sound;

    public void PlayeSound()
    {
        Debug.Log(name + ":" + sound);
    }
}

