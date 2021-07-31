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
                Debug.LogError("instance�� �����ϴ�.");
                return null;
            }

            return instance;
        }
    }

    private UIManager uIManager = null;

    private bool isGameOver = false;

    public static bool IsGameOver
    {
        get
        {
            return Instance.isGameOver;
        }

        private set
        {
            Instance.isGameOver = value;
        }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("�ټ��� ���ӸŴ����� ����ǰ� �ֽ��ϴ�.");
            Destroy(this);
            return;
        }

        instance = this;

        uIManager = FindObjectOfType<UIManager>();
    }

    public static void GameOver()
    {
        if (IsGameOver) return;

        IsGameOver = true;

        Instance.uIManager.GameOver();
    }

    public void ReStart()
    {
        DOTween.CompleteAll();
        DOTween.KillAll();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
