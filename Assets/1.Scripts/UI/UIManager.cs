using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public CanvasGroup gameOverPanel;
    public CanvasGroup gameStartPanel;
    public TextMeshProUGUI gameStartTmp;
    public float fadeDuration = 2.0f;
    public float fadeDuration2 = 2.0f;


    private void Start()
    {
        StartCoroutine(FadeOutPanel());

    }
    private void Update()
    {
        int curretStage = PlayerPrefs.GetInt("curretStage", 1);
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName == "Boss")
        {
            gameStartTmp.text = "강한 기사";
        }
        else if(currentSceneName == "Boss1")
        {
            gameStartTmp.text = "똑똑한 마법사";
        }
        else
        {

        gameStartTmp.text = $"{curretStage} Stage";
        }
    }
    public void ShowGameOver()
    {
        StartCoroutine(FadeInPanel());
    }

    private IEnumerator FadeInPanel()
    {
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            gameOverPanel.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            yield return null;
        }

        gameOverPanel.alpha = 1f;
        gameOverPanel.interactable = true;
        gameOverPanel.blocksRaycasts = true;
    }

    private IEnumerator FadeOutPanel()
    {
        float timer = 0f;

        gameStartPanel.interactable = false;
        gameStartPanel.blocksRaycasts = false;

        while (timer < fadeDuration2)
        {
            timer += Time.deltaTime;
            gameStartPanel.alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration2);
            yield return null;
        }

        gameStartPanel.alpha = 0f;
    }
}
