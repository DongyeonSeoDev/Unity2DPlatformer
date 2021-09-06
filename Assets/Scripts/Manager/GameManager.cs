using System.Text;
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
                Debug.LogError("instance가 없습니다.");
                return null;
            }

            return instance;
        }
    }

    private UIManager uIManager = null;
    private StringBuilder sb = new StringBuilder(8);

    private float time = 0f;

    public Dictionary<int, GameObject> stages = new Dictionary<int, GameObject>();

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

        uIManager = FindObjectOfType<UIManager>();
    }

    private void Start()
    {
        stageReset += () =>
        {
            isPause = false;
            isEnemyStop = false;
            time = 0f;
        };
    }

    private void Update()
    {
        if (isPause) return;

        if (isStageSelection)
        {
            return;
        }

        time += Time.deltaTime;
    }

    private void OnDisable()
    {
        if (stages.ContainsKey(currentStage))
        {
            if (stages[currentStage] != null && stages[currentStage].activeSelf)
            {
                stages[currentStage].SetActive(false);
            }
        }
    }

    public void ReStart()
    {
        stageReset();
    }

    public void StageSelection()
    {
        stages[currentStage].SetActive(false);
        currentStage = 0;
        stages[currentStage].SetActive(true);
        isStageSelection = true;
        uIManager.StageSelection();
    }

    public void Exit()
    {
        if (stages[currentStage].activeSelf)
        {
            stages[currentStage].SetActive(false);
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
        time = 0f;

        uIManager.StageStart();
    }
    public void GameEnd(eGameStates state)
    {
        if (isPause) return;

        isPause = true;
        Instance.isEnemyStop = true;

        uIManager.GameEnd(state);
    }

    private string timeCheck(int time)
    {
        if (time < 10)
        {
            return "0" + time;
        }

        return time.ToString();
    }

    public string TimeDisplay()
    {
        int minute = (int)time / 60;
        int second = (int)time - minute * 60;
        int millisecond = (int)((time - (minute * 60 + second)) * 100);

        sb.Remove(0, sb.Length);
        sb.Append(timeCheck(minute));
        sb.Append(':');
        sb.Append(timeCheck(second));
        sb.Append(':');
        sb.Append(timeCheck(millisecond));

        return sb.ToString();
    }
}
