using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StageDoor : MonoBehaviour
{
    [SerializeField] private GameObject stageText = null;
    [SerializeField] private Vector3 currentStageText = Vector3.zero;
    [SerializeField] private int stageNumber = 0;

    private Tween currentTween = null;

    private void Start()
    {
        currentTween = stageText.transform.DOScale(Vector3.zero, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        currentTween.Complete();
        currentTween.Kill();
        currentTween = stageText.transform.DOScale(currentStageText, 0.5f)
            .OnComplete(() => GameManager.Instance.currentStage = stageNumber);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!GameManager.Instance.isStageSelection)
        {
            return;
        }

        currentTween.Complete();
        currentTween.Kill();
        currentTween = stageText.transform.DOScale(Vector3.zero, 0.5f);
        GameManager.Instance.currentStage = 0;
    }
}
