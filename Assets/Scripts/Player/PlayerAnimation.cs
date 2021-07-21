using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private PlayerInput playerInput = null;
    private SpriteRenderer spriteRenderer = null;
    private Animator animator = null;

    [SerializeField] private Sprite jumpSprite = null;
    [SerializeField] private Sprite downSprite = null;

    private readonly int hashIsMove = Animator.StringToHash("isMove");
    private bool isJump = false;
    private bool isDown = false;

    private void Awake()
    {
        playerInput = FindObjectOfType<PlayerInput>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetBool(hashIsMove, playerInput.xMove != 0);
    }

    public void JumpAnimation()
    {
        if (isJump) return;

        isJump = true;
        animator.enabled = false;
        spriteRenderer.sprite = jumpSprite;
    }

    public void DownAnimation()
    {
        if (isDown) return;

        isDown = true;
        animator.enabled = false;
        spriteRenderer.sprite = downSprite;
    }

    public void AnimationStart()
    {
        if (animator.enabled) return;

        animator.enabled = true;
        isJump = false;
        isDown = false;
    }
}
