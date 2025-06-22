using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public CanvasGroup gameOverPanel;
    public float fadeDuration = 2.0f;

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
}
