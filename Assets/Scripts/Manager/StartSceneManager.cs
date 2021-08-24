using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    [SerializeField] private Button startButton = null;
    [SerializeField] private Button exitButton = null;

    private void Awake()
    {
        startButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("StageScene");
        });

        exitButton.onClick.AddListener(() =>
        {
            GameManager.Instance.Exit();
        });
    }
}
