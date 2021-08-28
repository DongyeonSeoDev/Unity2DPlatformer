using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectionManager : MonoBehaviour
{
    [SerializeField] private GameObject[] stageName = null;
    [SerializeField] private Vector3[] stageStartPosition = null;

    private PlayerMove playerMove = null;

    private GameManager gameManager = null;

    private void Awake()
    {
        playerMove = FindObjectOfType<PlayerMove>();
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void Update()
    {
        if (playerMove.isJump && gameManager.currentStage != 0)
        {
            Instantiate(stageName[gameManager.currentStage], Vector3.zero, Quaternion.identity);
            playerMove.ResetMove(stageStartPosition[gameManager.currentStage]);

            gameManager.StageStart();
            stageName[0].SetActive(false);
        }
    }
}
