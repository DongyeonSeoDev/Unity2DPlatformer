using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private float blinkTransitionTime = 5f;

    public float xMove = 0f;

    public bool isJump = false;
    public bool isBlink = false;
    public bool isPause = false;
    public bool isStagetSart = false;

    public bool isRightButtonClick = false;
    public bool isLeftButtonClick = false;
    public bool isJumpButtonClick = false;
    public bool isPauseButtonClick = false;
    public bool isStageStartButtonClick = false;

    private float lastInputTime = 0f;

    private void Start()
    {
        lastInputTime = Time.time;
    }

    private void Update()
    {
        isPause = Input.GetButtonDown("Cancel");

        if (GameManager.isPause)
        {
            xMove = 0;
            isJump = false;
            isBlink = false;
            isStagetSart = false;
            return;
        }

        xMove = Input.GetAxisRaw("Horizontal");
        isJump = Input.GetButtonDown("Jump");
        isStagetSart = Input.GetKeyDown(KeyCode.Z);

        if (isRightButtonClick)
        {
            xMove = Mathf.Clamp(xMove + 1, -1, 1);
        }

        if (isLeftButtonClick)
        {
            xMove = Mathf.Clamp(xMove - 1, -1, 1);
        }

        if (isJumpButtonClick)
        {
            isJump = true;
            isJumpButtonClick = false;
        }

        if (isPauseButtonClick)
        {
            isPause = true;
            isPauseButtonClick = false;
        }

        if (isStageStartButtonClick)
        {
            isStagetSart = true;
            isStageStartButtonClick = false;
        }

        if (xMove != 0 || isJump || isPause || isStagetSart)
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

    public void ResetInput()
    {
        xMove = 0f;
        isJump = false;
        isBlink = false;
        isPause = false;
        isStagetSart = false;
        lastInputTime = Time.time;
    }
}