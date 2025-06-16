using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoo : MonoBehaviour
{
    private void Start()
    {
        Animal tom = new Animal();

        tom.name = "��";
        tom.sound = "�Ŀ�";

        Animal jerry = new Animal();

        jerry.name = "����";
        jerry.sound = "����";

        tom.PlayeSound();
        jerry.PlayeSound();

        jerry = tom;
        jerry.name = "��Ű";

        tom.PlayeSound();
        jerry.PlayeSound();
    }
}
