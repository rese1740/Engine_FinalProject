using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoo : MonoBehaviour
{
    private void Start()
    {
        Animal tom = new Animal();

        tom.name = "Åè";
        tom.sound = "³Ä¿Ë";

        Animal jerry = new Animal();

        jerry.name = "Á¦¸®";
        jerry.sound = "ÂïÂï";

        tom.PlayeSound();
        jerry.PlayeSound();

        jerry = tom;
        jerry.name = "¹ÌÅ°";

        tom.PlayeSound();
        jerry.PlayeSound();
    }
}
