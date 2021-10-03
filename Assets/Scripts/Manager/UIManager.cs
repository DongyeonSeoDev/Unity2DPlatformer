using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    private static UIManager instance = null;

    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.Log("UIManager instance가 없습니다.");
                return null;
            }

            return instance;
        }
    }

    [Header("DoorSign")]
    public Sprite clearDoorSign = null;
    public Sprite openDoorSign = null;

    [Header(" ")]
    public CanvasGroup mainCanvasGroup = null;
    public CanvasGroup pauseCanvasGroup = null;
    public CanvasGroup gameOverCanvasGroup = null;
    public CanvasGroup gameClearCanvasGroup = null;

    public Button coutinueButton = null;
    public Button[] reStartButtons = null;
    public Button[] stageSelectionButtons = null;
    public Button[] exitButtons = null;

    public Text timeText = null;
    public Text gameClearTimeText = null;

    public EventTrigger rightButton = null;
    public EventTrigger leftButton = null;

    public Button jumpButton = null;
    public Button pauseButton = null;
    public Button stageStartButton = null;

    public Tween stageStartButtonTween = null;

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
        if (instance != null)
        {
            Debug.LogError("다수의 UI매니저가 실행되고 있습니다.");
            Destroy(this);
            return;
        }

        instance = this;

        playerInput = FindObjectOfType<PlayerInput>();

        coutinueButton.onClick.AddListener(() =>
        {
            if (!isPauseTweenComplete)
            {
                return;
            }

            Pause();
        });

        foreach (Button reStartButton in reStartButtons)
        {
            reStartButton.onClick.AddListener(() =>
            {
                if (isClick || !isPauseTweenComplete)
                {
                    return;
                }

                Time.timeScale = 1f;
                isClick = true;

                if (isPause)
                {
                    ResetUI(pauseCanvasGroup);
                }
                else
                {
                    ResetUI(gameEndCanvasGroup);
                }
            });
        }

        foreach (Button stageSelectionButton in stageSelectionButtons)
        {
            stageSelectionButton.onClick.AddListener(() =>
            {
                if (isClick || !isPauseTweenComplete)
                {
                    return;
                }

                isClick = true;
                Time.timeScale = 1f;
                gameManager.StageSelection();

                if (isPause)
                {
                    ResetUI(pauseCanvasGroup);
                }
                else
                {
                    ResetUI(gameEndCanvasGroup);
                }
            });
        }

        foreach (Button exitButton in exitButtons)
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
        }

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
        stageStartButton.onClick.AddListener(() => playerInput.isStageStartButtonClick = true);

        stageStartButtonTween = stageStartButton.transform.DOScale(Vector3.zero, 0f);
    }

    private void Start()
    {
        gameManager = GameManager.Instance;

        if (gameManager.isStageSelection)
        {
            StageSelection();
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

    public void UpdateUI(Stage[] stages)
    {
        bool isUseOpenSign = false;

        foreach (Stage stage in stages)
        {
            if (stage.stageNumber == 0)
            {
                continue;
            }

            if (stage.highScore > 0)
            {
                GameManager.Instance.SetStageText(stage);
            }

            if (stage.isStageClear)
            {
                stage.doorSignSpriteRender.sprite = clearDoorSign;
            }
            else if (!isUseOpenSign && !stage.isStageClear && stage.stageNumber != 0)
            {
                stage.doorSignSpriteRender.sprite = openDoorSign;
                isUseOpenSign = true;
            }

            if (!stage.isAnimationPlay)
            {
                stage.lockDoorSign.SetActive(false);
            }
        }
    }

    public void StageSelection()
    {
        timeIcon.SetActive(false);
        isStageSelection = true;
    }

    public void ResetUI(CanvasGroup currentCanvasGroup)
    {
        isClick = false;
        isPause = false;

        mainCanvasGroup.interactable = true;
        mainCanvasGroup.blocksRaycasts = true;
        mainCanvasGroup.alpha = 1f;

        currentCanvasGroup.alpha = 0f;
        currentCanvasGroup.interactable = false;
        currentCanvasGroup.blocksRaycasts = false;

        gameManager.ReStart();
    }

    public void StageStart()
    {
        timeIcon.SetActive(true);
        isStageSelection = false;
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
            gameManager.currentStageDoor.Clear();
            gameManager.stages[gameManager.currentStage].isStageClear = true;
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

        LocalSaveManager.Save(gameManager.stages);
    }
}
