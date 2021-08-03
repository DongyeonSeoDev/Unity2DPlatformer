using System.Collections;
using System.Collections.Generic;
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

    public static bool isPause = false;

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

    public static void GameOver()
    {
        if (isPause) return;

        isPause = true;

        Instance.uIManager.GameOver();
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
}
