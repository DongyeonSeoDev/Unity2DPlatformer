using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class StageSelectionManager : MonoBehaviour
{
    [HideInInspector]
    public Stage[] stages = null;

    private PlayerMove playerMove = null;
    private PlayerInput playerInput = null;
    private GameManager gameManager = null;

    private Vector3 targetLockDoorSignScale = new Vector3(0f, 1f, 1f);

    private int currentStage = 0;

    private void Awake()
    {
        playerMove = FindObjectOfType<PlayerMove>();
    }

    private void Start()
    {
        playerInput = playerMove.playerInput;

        gameManager = GameManager.Instance;
        stages = gameManager.stages;
        gameManager.stageReset += () => 
        {
            playerMove.ResetMove(stages[gameManager.currentStage].stageStartPosition);
            playerInput.ResetInput();
        };
    }

    private void Update()
    {
        if (!gameManager.isStageSelection)
        {
            return;
        }

        if (playerInput.isStagetSart && gameManager.currentStage != 0)
        {
            if (!stages[gameManager.currentStage - 1].isStageClear)
            {
                return;
            }

            currentStage = gameManager.currentStage;

            if (stages[currentStage].isAnimationPlay)
            {
                stages[currentStage].isAnimationPlay = false;
                gameManager.isStageSelection = false;
                GameManager.isPause = true;

                stages[currentStage].lockDoorSign.transform.DOScale(targetLockDoorSignScale, 1f).OnComplete(() =>
                {
                    GameManager.isPause = false;
                    stages[currentStage].lockDoorSign.SetActive(false);
                    StartStage();
                });
            }
            else
            {
                StartStage();
            }
        }
    }

    private void StartStage()
    {
        if (stages[currentStage].stageObject == null)
        {
            stages[currentStage].stageObject = Instantiate(stages[currentStage].stagePrefab, Vector3.zero, Quaternion.identity);
        }
        else
        {
            stages[currentStage].stageObject.SetActive(true);
        }

        gameManager.ReStart();
        gameManager.StageStart();

        stages[0].stageObject.SetActive(false);
    }
}
