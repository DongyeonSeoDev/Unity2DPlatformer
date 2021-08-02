using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingThornAnimation : MonoBehaviour
{
    [SerializeField] private Vector3 addRotation = Vector3.zero;

    private Quaternion currentRotation = Quaternion.identity;
    private Quaternion targetRotation = Quaternion.identity;


    private void Update()
    {
        if (GameManager.isPause)
        {
            return;
        }

        currentRotation = transform.rotation;
        targetRotation.eulerAngles = currentRotation.eulerAngles + addRotation * Time.deltaTime;
        transform.rotation = targetRotation;
    }
}
