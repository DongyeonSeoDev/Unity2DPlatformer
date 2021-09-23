using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class StageDoor : MonoBehaviour
{
    [SerializeField] private int stageNumber = 0;

    private Stage[] stages = null;

    private GameManager gameManager = null;
    private UIManager uIManager = null;

    private Tween currentTween = null;

    private Vector3 currentStageTextSize = new Vector3(0.5f, 0.5f, 0.5f);
    private Vector3 targetLockDoorSignScale = new Vector3(0f, 1f, 1f);

    private void Start()
    {
        gameManager = GameManager.Instance;
        uIManager = UIManager.Instance;

        stages = gameManager.stages;
        currentTween = stages[stageNumber].stageSign.transform.DOScale(Vector3.zero, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!gameManager.stages[stageNumber - 1].isStageClear)
        {
            return;
        }

        currentTween.Complete();
        currentTween.Kill();
        currentTween = stages[stageNumber].stageSign.transform.DOScale(currentStageTextSize, 0.3f)
            .OnComplete(() => 
            {
                gameManager.currentStage = stageNumber;
                gameManager.currentStageDoor = this;
            });

        uIManager.stageStartButtonTween.Complete();
        uIManager.stageStartButtonTween = uIManager.stageStartButton.transform.DOScale(Vector3.one, 0.3f);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!gameManager.isStageSelection || !gameManager.stages[stageNumber - 1].isStageClear)
        {
            return;
        }

        currentTween.Complete();
        currentTween.Kill();
        currentTween = stages[stageNumber].stageSign.transform.DOScale(Vector3.zero, 0.3f);
        gameManager.currentStage = 0;

        uIManager.stageStartButtonTween.Complete();
        uIManager.stageStartButtonTween = uIManager.stageStartButton.transform.DOScale(Vector3.zero, 0.3f);
    }

    private void OnDisable()
    {
        try
        {
            stages[stageNumber].stageSign.transform.localScale = Vector3.zero;
            uIManager.stageStartButton.transform.localScale = Vector3.zero;
        }
        catch (Exception e)
        {
            Debug.Log(e);
            return;
        }
    }

    public void Clear()
    {
        if (stages[stageNumber].isStageClear)
        {
            return;
        }

        stages[stageNumber].isStageClear = true;
        stages[stageNumber].doorSignSpriteRender.sprite = uIManager.clearDoorSign;

        if (stageNumber + 1 < stages.Length)
        {
            DoorOpen(stages[stageNumber + 1]);
        }
    }

    private void DoorOpen(Stage stage)
    {
        stage.doorSignSpriteRender.sprite = uIManager.openDoorSign;
        stage.lockDoorSign.transform.DOScale(targetLockDoorSignScale, 1f).OnComplete(() =>
        {
            stage.lockDoorSign.SetActive(false);
        }); 
    }
}
