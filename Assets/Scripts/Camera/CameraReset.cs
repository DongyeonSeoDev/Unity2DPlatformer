using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraReset : MonoBehaviour
{
    [SerializeField] private Vector3[] cameraStartPosition = null;
    [SerializeField] private float[] cameraResetTime = null;

    private Camera mainCamera = null;
    private Camera temporaryCamera = null;

    private void Awake()
    {
        mainCamera = Camera.main;
        temporaryCamera = GetComponent<Camera>();
    }

    private void Start()
    {
        GameManager.Instance.stageReset += () =>
        {
            transform.position = cameraStartPosition[GameManager.Instance.currentStage];
            temporaryCamera.enabled = true;
            mainCamera.enabled = false;
            Invoke("Complete", cameraResetTime[GameManager.Instance.currentStage]);
        };
    }

    private void Complete()
    {
        mainCamera.enabled = true;
        temporaryCamera.enabled = false;
    }
}
