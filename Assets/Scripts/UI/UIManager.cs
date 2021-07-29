using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public CanvasGroup gameOverCanvasGroup = null;
    public Button reStart = null;
    public Button exit = null;

    private void Awake()
    {
        reStart.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;

            gameOverCanvasGroup.DOFade(0.5f, 0.1f).OnComplete(() =>
            {
                DOTween.KillAll();
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            });
        });

        exit.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }

    public void GameOver()
    {
        gameOverCanvasGroup.DOFade(1f, 0.2f).OnComplete(() =>
        {
            Time.timeScale = 0f;
            gameOverCanvasGroup.interactable = true;
            gameOverCanvasGroup.blocksRaycasts = true;
        });
    }
}
