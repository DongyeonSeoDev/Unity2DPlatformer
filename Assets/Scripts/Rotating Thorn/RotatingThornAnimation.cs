using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RotatingThornAnimation : MonoBehaviour
{
    [SerializeField] private Vector3 addRotation = Vector3.zero;

    private Quaternion currentRotation = Quaternion.identity;
    private Quaternion targetRotation = Quaternion.identity;

    private Quaternion startRotation;

    private void Start()
    {
        startRotation = transform.rotation;
    }

    private void Update()
    {
        if (GameManager.isPause)
        {
            return;
        }
    }

    private void FixedUpdate()
    {
        currentRotation = transform.rotation;
        targetRotation.eulerAngles = currentRotation.eulerAngles + addRotation * Time.deltaTime;
        transform.rotation = targetRotation;
    }

    private void OnEnable()
    {
        GameManager.Instance.stageReset += RotatingThornReset;
    }

    private void OnDisable()
    {
        try
        {
            GameManager.Instance.stageReset -= RotatingThornReset;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    private void RotatingThornReset()
    {
        transform.rotation = startRotation;
    }
}
