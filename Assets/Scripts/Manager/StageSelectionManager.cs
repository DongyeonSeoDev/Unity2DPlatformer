using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectionManager : MonoBehaviour
{
    [SerializeField] private GameObject[] stageObject = null;
    [SerializeField] private Vector3[] stageStartPosition = null;

    private Dictionary<int, GameObject> stages;
    private PlayerMove playerMove = null;
    private GameManager gameManager = null;

    private int currentStage = 0;

    private void Awake()
    {
        playerMove = FindObjectOfType<PlayerMove>();
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.stageReset += () => playerMove.ResetMove(stageStartPosition[gameManager.currentStage]);
        stages = gameManager.stages;

        stages.Add(0, stageObject[0]);
    }

    private void Update()
    {
        if (!gameManager.isStageSelection)
        {
            return;
        }

        #region DEBUG

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            gameManager.currentStage = 2;
            playerMove.isJump = true;
        }

        #endregion

        if (playerMove.isJump && gameManager.currentStage != 0)
        {
            currentStage = gameManager.currentStage;

            if (!stages.ContainsKey(currentStage))
            {
                stages.Add(currentStage, Instantiate(stageObject[currentStage], Vector3.zero, Quaternion.identity));
            }
            else
            {
                stages[currentStage].SetActive(true);
            }

            gameManager.ReStart();
            gameManager.StageStart();

            stageObject[0].SetActive(false);
        }
    }
}
