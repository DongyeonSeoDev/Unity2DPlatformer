using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectionManager : MonoBehaviour
{
    public Stage[] stages = null;

    private PlayerMove playerMove = null;
    private PlayerInput playerInput = null;
    private GameManager gameManager = null;

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
}
