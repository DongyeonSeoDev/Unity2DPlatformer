using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectionManager : MonoBehaviour
{
    [SerializeField] private GameObject[] stageName = null;

    private PlayerInput playerInput = null;

    private void Awake()
    {
        playerInput = FindObjectOfType<PlayerInput>();
    }

    private void Update()
    {
        if (playerInput.isJump && GameManager.Instance.currentStage != 0)
        {
            Instantiate(stageName[GameManager.Instance.currentStage + 1], Vector3.zero, Quaternion.identity);
            stageName[0].SetActive(false);
        }
    }
}
