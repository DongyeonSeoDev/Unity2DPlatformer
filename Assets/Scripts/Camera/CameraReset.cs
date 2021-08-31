using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraReset : MonoBehaviour
{
    [SerializeField] private Camera mainCamera = null;
    [SerializeField] private Vector3[] cameraStartPosition = null;

    private Camera temporaryCamera = null;

    private void Awake()
    {
        temporaryCamera = GetComponent<Camera>();
    }

    private void Start()
    {
        GameManager.Instance.stageReset += () =>
        {
            transform.position = cameraStartPosition[GameManager.Instance.currentStage];
            temporaryCamera.enabled = true;
            mainCamera.enabled = false;
            Invoke("Complete", 1f);
        };
    }

    private void Complete()
    {
        mainCamera.enabled = true;
        temporaryCamera.enabled = false;
    }
}
