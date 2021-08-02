using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public CanvasGroup gameOverCanvasGroup = null;
    public CanvasGroup pauseCanvasGroup = null;

    public Button gameOverReStart = null;
    public Button gameOverExit = null;

    public Button pauseContinue = null;
    public Button pauseReStart = null;
    public Button pauseExit = null;

    private GameManager gameManager = null;
    private PlayerInput playerInput = null;

    private bool isPause = false;
    private bool isPauseTweenComplete = true;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        playerInput = FindObjectOfType<PlayerInput>();

        gameOverReStart.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;

            gameOverCanvasGroup.DOFade(0.5f, 0.1f).OnComplete(() =>
            {
                gameManager.ReStart();
            });
        });

        gameOverExit.onClick.AddListener(() =>
        {
            gameManager.Exit();
        });

        pauseContinue.onClick.AddListener(() =>
        {
            if (!isPauseTweenComplete)
            {
                return;
            }

            Pause();
        });

        pauseReStart.onClick.AddListener(() =>
        {
            if (!isPauseTweenComplete)
            {
                return;
            }

            Time.timeScale = 1f;

            pauseCanvasGroup.DOFade(0.5f, 0.2f).OnComplete(() =>
            {
                gameManager.ReStart();
            });
        });

        pauseExit.onClick.AddListener(() =>
        {
            if (!isPauseTweenComplete)
            {
                return;
            }

            gameManager.Exit();
        });
    }

    private void Update()
    {
        if (playerInput.isPause)
        {
            Pause();
        }
    }

    private void Pause()
    {
        if (!isPauseTweenComplete || (GameManager.isPause && !isPause))
        {
            return;
        }

        isPauseTweenComplete = false;
        isPause = !isPause;
        GameManager.isPause = isPause;
        
        if (!isPause)
        {
            Time.timeScale = 1f;
        }
        else
        {
            pauseCanvasGroup.interactable = true;
            pauseCanvasGroup.blocksRaycasts = true;
        }

        pauseCanvasGroup.DOFade(isPause ? 1f : 0f, 0.5f).OnComplete(() => 
        {
            if (isPause)
            {
                Time.timeScale = 0f;
            }
            else
            {
                pauseCanvasGroup.interactable = false;
                pauseCanvasGroup.blocksRaycasts = false;
            }

            isPauseTweenComplete = true;
        });
    }

    public void GameOver()
    {
        gameOverCanvasGroup.DOFade(1f, 0.2f).OnComplete(() =>
        {
            Time.timeScale = 0f;

            pauseCanvasGroup.alpha = 0;
            pauseCanvasGroup.interactable = false;
            pauseCanvasGroup.blocksRaycasts = false;

            gameOverCanvasGroup.interactable = true;
            gameOverCanvasGroup.blocksRaycasts = true;
        });
    }
}
