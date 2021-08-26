using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public CanvasGroup gameOverCanvasGroup = null;
    public CanvasGroup gameClearCanvasGroup = null;
    public CanvasGroup pauseCanvasGroup = null;
    public CanvasGroup mainCanvasGroup = null;

    public List<Button> exitButtons = null;
    public List<Button> reStartButtons = null;

    public Button coutinueButton = null;

    public Text timeText = null;
    public Text gameClearTimeText = null;

    public EventTrigger rightButton = null;
    public EventTrigger leftButton = null;

    public Button jumpButton = null;
    public Button pauseButton = null;

    public GameObject timeIcon = null;

    private Dictionary<string, EventTrigger.Entry> eventTriggerDictionary = new Dictionary<string, EventTrigger.Entry>();

    private GameManager gameManager = null;
    private PlayerInput playerInput = null;
    private CanvasGroup gameEndCanvasGroup = null;

    private bool isPause = false;
    private bool isPauseTweenComplete = true;
    private bool isClick = false;
    private bool isStageSelection = false;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        playerInput = FindObjectOfType<PlayerInput>();

        exitButtons.ForEach((exitButton) =>
        {
            exitButton.onClick.AddListener(() =>
            {
                if (isClick || !isPauseTweenComplete)
                {
                    return;
                }

                isClick = true;
                gameManager.Exit();
            });
        });

        reStartButtons.ForEach((reStartButton) =>
        {
            reStartButton.onClick.AddListener(() =>
            {
                if (!isPauseTweenComplete || isClick)
                {
                    return;
                }

                Time.timeScale = 1f;
                isClick = true;

                if (isPause)
                {
                    pauseCanvasGroup.DOFade(0f, 0.2f).OnComplete(() =>
                    {
                        gameManager.ReStart();
                    });
                }
                else
                {
                    gameEndCanvasGroup.DOFade(0f, 0.2f).OnComplete(() =>
                    {
                        gameManager.ReStart();
                    });
                }
            });
        });
        
        coutinueButton.onClick.AddListener(() =>
        {
            if (!isPauseTweenComplete)
            {
                return;
            }

            Pause();
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

        jumpButton.onClick.AddListener(() => playerInput.isJumpButtonClick = true);
        pauseButton.onClick.AddListener(() => playerInput.isPauseButtonClick = true);
    }

    private void Start()
    {
        if (gameManager.isStageSelection)
        {
            timeIcon.SetActive(false);
            isStageSelection = true;
        }
    }

    private void Update()
    {
        if (playerInput.isPause)
        {
            Pause();
        }

        if (isStageSelection)
        {
            return;
        }

        timeText.text = gameManager.TimeDisplay();
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

            mainCanvasGroup.interactable = true;
            mainCanvasGroup.blocksRaycasts = true;
        }
        else
        {
            pauseCanvasGroup.interactable = true;
            pauseCanvasGroup.blocksRaycasts = true;
        }
        
        mainCanvasGroup.DOFade(isPause ? 0f : 1f, 0.5f);

        pauseCanvasGroup.DOFade(isPause ? 1f : 0f, 0.5f).OnComplete(() => 
        {
            if (isPause)
            {
                Time.timeScale = 0f;

                mainCanvasGroup.alpha = 0f;
                mainCanvasGroup.interactable = false;
                mainCanvasGroup.blocksRaycasts = false;
            }
            else
            {
                mainCanvasGroup.alpha = 1f;

                pauseCanvasGroup.interactable = false;
                pauseCanvasGroup.blocksRaycasts = false;
            }

            isPauseTweenComplete = true;
        });
    }

    public void GameEnd(eGameStates state)
    {
        mainCanvasGroup.DOFade(0f, 0.2f);

        if (state == eGameStates.gameOver)
        {
            gameEndCanvasGroup = gameOverCanvasGroup;
        }
        else if (state == eGameStates.gameClear)
        {
            gameClearTimeText.text = gameManager.TimeDisplay();
            gameEndCanvasGroup = gameClearCanvasGroup;
        }

        gameEndCanvasGroup.DOFade(1f, 0.2f).OnComplete(() =>
        {
            Time.timeScale = 0f;

            mainCanvasGroup.alpha = 0;
            mainCanvasGroup.interactable = false;
            mainCanvasGroup.blocksRaycasts = false;

            pauseCanvasGroup.alpha = 0;
            pauseCanvasGroup.interactable = false;
            pauseCanvasGroup.blocksRaycasts = false;
            isPauseTweenComplete = true;

            gameEndCanvasGroup.interactable = true;
            gameEndCanvasGroup.blocksRaycasts = true;
        });
    }
}
