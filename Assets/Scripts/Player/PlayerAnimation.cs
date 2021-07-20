using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private PlayerInput playerInput = null;
    private Animator animator = null;

    private readonly int hashIsMove = Animator.StringToHash("isMove");

    private void Awake()
    {
        playerInput = FindObjectOfType<PlayerInput>();
        animator = FindObjectOfType<Animator>();
    }

    private void Update()
    {
        animator.SetBool(hashIsMove, playerInput.xMove != 0);
    }
}
