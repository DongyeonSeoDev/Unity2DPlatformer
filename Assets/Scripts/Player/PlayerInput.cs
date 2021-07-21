using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float xMove { get; private set; } = 0f;
    public bool isJump { get; private set; } = false;

    private void Update()
    {
        xMove = Input.GetAxisRaw("Horizontal");
        isJump = Input.GetButtonDown("Jump");
    }
}
