using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StageDoor : MonoBehaviour
{
    [SerializeField] private int stageNumber = 0;

    private Stage[] stages = null;

    private Tween currentTween = null;

    private Vector3 currentStageTextSize = new Vector3(0.5f, 0.5f, 0.5f);

    private void Start()
    {
        stages = GameManager.Instance.stages;
        currentTween = stages[stageNumber].stageSign.transform.DOScale(Vector3.zero, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GameManager.Instance.stages[stageNumber - 1].isStageClear)
        {
            return;
        }

        currentTween.Complete();
        currentTween.Kill();
        currentTween = stages[stageNumber].stageSign.transform.DOScale(currentStageTextSize, 0.3f)
            .OnComplete(() => 
            {
                GameManager.Instance.currentStage = stageNumber;
                GameManager.Instance.currentStageDoor = this;
            });
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!GameManager.Instance.isStageSelection || !GameManager.Instance.stages[stageNumber - 1].isStageClear)
        {
            return;
        }

        currentTween.Complete();
        currentTween.Kill();
        currentTween = stages[stageNumber].stageSign.transform.DOScale(Vector3.zero, 0.3f);
        GameManager.Instance.currentStage = 0;
    }

    private void OnDisable()
    {
        stages[stageNumber].stageSign.transform.localScale = Vector3.zero;
    }

    public void Clear()
    {
        if (stages[stageNumber].isStageClear)
        {
            return;
        }

        stages[stageNumber].isStageClear = true;
        stages[stageNumber].doorSignSpriteRender.sprite = UIManager.Instance.clearDoorSign;

        if (stageNumber + 1 < stages.Length)
        {
            stages[stageNumber + 1].doorSignSpriteRender.sprite = UIManager.Instance.openDoorSign;
        }
    }
}
