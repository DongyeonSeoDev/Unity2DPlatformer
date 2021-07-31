using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private float blinkTransitionTime = 5f;

    public float xMove { get; private set; } = 0f;
    public bool isJump { get; private set; } = false;
    public bool isBlink { get; private set; } = false;

    private float lastInputTime = 0f;

    private void Start()
    {
        lastInputTime = Time.time;
    }

    private void Update()
    {
        if (GameManager.IsGameOver)
        {
            xMove = 0;
            isJump = false;
            isBlink = false;
            return;
        }

        xMove = Input.GetAxisRaw("Horizontal");
        isJump = Input.GetButtonDown("Jump");

        if (xMove != 0 || isJump)
        {
            lastInputTime = Time.time;
        }

        if (Time.time - lastInputTime >= blinkTransitionTime)
        {
            isBlink = true;
        }
        else
        {
            isBlink = false;
        }
    }
}
