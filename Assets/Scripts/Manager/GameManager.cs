using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

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

    public static bool isPause = false;

    public bool isEnemyStop = false;

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

    private void Update()
    {
        if (isPause) return;

        time += Time.deltaTime;
    }

    public static void GameOver()
    {
        if (isPause) return;

        isPause = true;
        Instance.isEnemyStop = true;

        Instance.uIManager.GameOver();
    }

    public void GameClear()
    {
        if (isPause) return;

        isPause = true;
        Instance.isEnemyStop = true;

        Instance.uIManager.GameClear();
    }

    public void ReStart()
    {
        DOTween.KillAll();

        isPause = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Exit()
    {
        Application.Quit();
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
