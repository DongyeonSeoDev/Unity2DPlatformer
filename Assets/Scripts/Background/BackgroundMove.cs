using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMove : MonoBehaviour
{
    [SerializeField] private float limitX = 0f;

    [SerializeField] private Vector3 speed = Vector3.zero;
    [SerializeField] private Vector3 startPosition = Vector3.zero;

    private void Update()
    {
        transform.position += speed * Time.deltaTime;

        if (transform.position.x < limitX)
        {
            transform.position = startPosition;
        }
    }
}
