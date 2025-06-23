using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    public void ReStart()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Lobby");
    }

}
