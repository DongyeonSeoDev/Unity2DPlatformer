using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraReset : MonoBehaviour
{
    [SerializeField] private Vector3 cameraStartPosition = Vector3.zero;

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
            transform.position = cameraStartPosition;
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
