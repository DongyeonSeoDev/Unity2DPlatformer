using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public CanvasGroup gameOverCanvasGroup = null;
    public CanvasGroup pauseCanvasGroup = null;
    public CanvasGroup buttonCanvasGroup = null;

    public Button gameOverReStart = null;
    public Button gameOverExit = null;

    public Button pauseContinue = null;
    public Button pauseReStart = null;
    public Button pauseExit = null;

    public EventTrigger rightButton = null;
    public EventTrigger leftButton = null;
    public Button jumpButton = null;

    private Dictionary<string, EventTrigger.Entry> eventTriggerDictionary = new Dictionary<string, EventTrigger.Entry>();

    private GameManager gameManager = null;
    private PlayerInput playerInput = null;

    private bool isPause = false;
    private bool isPauseTweenComplete = true;
    private bool isClick = false;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        playerInput = FindObjectOfType<PlayerInput>();

        gameOverReStart.onClick.AddListener(() =>
        {
            if (isClick)
            {
                return;
            }

            Time.timeScale = 1f;
            isClick = true;

            gameOverCanvasGroup.DOFade(0.5f, 0.1f).OnComplete(() =>
            {
                gameManager.ReStart();
            });
        });

        gameOverExit.onClick.AddListener(() =>
        {
            if (isClick)
            {
                return;
            }

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
            if (!isPauseTweenComplete && isClick)
            {
                return;
            }

            Time.timeScale = 1f;
            isClick = true;

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

        eventTriggerDictionary.Add("RightButtonDown", new EventTrigger.Entry());
        eventTriggerDictionary.Add("RightButtonUp", new EventTrigger.Entry());
        eventTriggerDictionary.Add("LeftButtonDown", new EventTrigger.Entry());
        eventTriggerDictionary.Add("LeftButtonUp", new EventTrigger.Entry());

        eventTriggerDictionary["RightButtonDown"].eventID = EventTriggerType.PointerDown;
        eventTriggerDictionary["RightButtonDown"].callback.AddListener((data) =>
        {
            playerInput.isRightButtonClick = true;
        });

        eventTriggerDictionary["RightButtonUp"].eventID = EventTriggerType.PointerUp;
        eventTriggerDictionary["RightButtonUp"].callback.AddListener((data) =>
        {
            playerInput.isRightButtonClick = false;
        });

        eventTriggerDictionary["LeftButtonDown"].eventID = EventTriggerType.PointerDown;
        eventTriggerDictionary["LeftButtonDown"].callback.AddListener((data) =>
        {
            playerInput.isLeftButtonClick = true;
        });

        eventTriggerDictionary["LeftButtonUp"].eventID = EventTriggerType.PointerUp;
        eventTriggerDictionary["LeftButtonUp"].callback.AddListener((data) =>
        {
            playerInput.isLeftButtonClick = false;
        });

        rightButton.triggers.Add(eventTriggerDictionary["RightButtonDown"]);
        rightButton.triggers.Add(eventTriggerDictionary["RightButtonUp"]);
        leftButton.triggers.Add(eventTriggerDictionary["LeftButtonDown"]);
        leftButton.triggers.Add(eventTriggerDictionary["LeftButtonUp"]);

        jumpButton.onClick.AddListener(() =>
        {
            playerInput.isJumpButtonClick = true;
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

        buttonCanvasGroup.DOFade(isPause ? 0f : 1f, 0.5f);

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
        buttonCanvasGroup.DOFade(0f, 0.2f);

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
