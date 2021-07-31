using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    private GameManager gameManager = null;

    public CanvasGroup gameOverCanvasGroup = null;
    public Button reStart = null;
    public Button exit = null;

    private void Awake()
    {
        gameManager = GameManager.Instance;

        reStart.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;

            gameOverCanvasGroup.DOFade(0.5f, 0.1f).OnComplete(() =>
            {
                gameManager.ReStart();
            });
        });

        exit.onClick.AddListener(() =>
        {
            gameManager.Exit();
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
