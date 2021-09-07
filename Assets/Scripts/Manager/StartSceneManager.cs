using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class StartSceneManager : MonoBehaviour
{
    [SerializeField] private Button startButton = null;

    [SerializeField] private GameObject uI = null;
    [SerializeField] private CanvasGroup uICanvasGroup = null;

    private bool isClick = false;

    private void Awake()
    {
        startButton.onClick.AddListener(() =>
        {
            if (isClick)
            {
                return;
            }

            isClick = true;

            uI.transform.DOMoveY(50f, 1f).SetRelative();
            uICanvasGroup.DOFade(0f, 1f).OnComplete(() => 
            {
                DOTween.CompleteAll();
                DOTween.KillAll();
                SceneManager.LoadScene("StageScene");
            });
        });
    }
}
