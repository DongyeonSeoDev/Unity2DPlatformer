using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float xMove = 0;

    private void Update()
    {
        xMove = Input.GetAxisRaw("Horizontal");
    }
}
