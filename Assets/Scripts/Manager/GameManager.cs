using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public enum eGameStates
{
    gameOver, gameClear
}

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.Log("GameManager instance가 없습니다.");
                return null;
            }

            return instance;
        }
    }

    public Stage[] stages = null;

    private UIManager uIManager = null;

    private TimeSpan timeSpan = TimeSpan.Zero;
    private float currentTime = 0f;

    public static bool isPause = false;

    public event Action stageReset;

    public StageDoor currentStageDoor = null;
    public int currentStage = 0;

    public bool isEnemyStop = false;
    public bool isStageSelection = true;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("다수의 게임매니저가 실행되고 있습니다.");
            Destroy(this);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        uIManager = UIManager.Instance;

        stageReset += () =>
        {
            isPause = false;
            isEnemyStop = false;
            currentTime = 0f;
        };

        LocalSaveManager.Load(stages);
        uIManager.UpdateUI(stages);
    }

    private void Update()
    {
        if (isPause) return;

        if (isStageSelection)
        {
            return;
        }

        currentTime += Time.deltaTime;
    }

    private void OnDisable()
    {
        if (stages[currentStage].stageObject != null && stages[currentStage].stageObject.activeSelf)
        {
            stages[currentStage].stageObject.SetActive(false);
        }
    }

    public void ReStart()
    {
        stageReset();
    }

    public void StageSelection()
    {
        stages[currentStage].stageObject.SetActive(false);
        currentStage = 0;
        stages[currentStage].stageObject.SetActive(true);
        isStageSelection = true;
        uIManager.StageSelection();
    }

    public void Exit()
    {
        LocalSaveManager.Save(stages);

        if (stages[currentStage].stageObject != null && stages[currentStage].stageObject.activeSelf)
        {
            stages[currentStage].stageObject.SetActive(false);
        }

#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void StageStart()
    {
        isStageSelection = false;
        currentTime = 0f;

        uIManager.StageStart();
        LocalSaveManager.Save(stages);
    }
    public void GameEnd(eGameStates state)
    {
        if (isPause) return;

        isPause = true;
        Instance.isEnemyStop = true;

        uIManager.GameEnd(state);
    }

    public string TimeDisplay(float time = -0.01f)
    {
        if (time == -0.01f)
        {
            timeSpan = TimeSpan.FromSeconds(currentTime);
        }
        else
        {
            timeSpan = TimeSpan.FromSeconds(time);
        }

        return $"{timeSpan.Minutes.ToString("00")}:{timeSpan.Seconds.ToString("00")}:{timeSpan.Milliseconds.ToString("000")}";
    }

    public void HighScoreCheck(Stage stage)
    {
        if (stage.highScore > currentTime || stage.highScore == 0)
        {
            stage.highScore = currentTime;
            SetStageText(stage);
        }
    }

    public void SetStageText(Stage stage)
    {
        stage.stageText.text = $"스테이지 {stage.stageNumber}\n최고기록: {TimeDisplay(stage.highScore)}";
    }
}
