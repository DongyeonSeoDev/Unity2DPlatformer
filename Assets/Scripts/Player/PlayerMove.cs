using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private Transform groundCheckTransform = null;
    [SerializeField] private Transform rightGroundCheckTransform = null;
    [SerializeField] private Transform leftGroundCheckTransform = null;

    [SerializeField] private Vector2 jumpforce = Vector2.zero;
    [SerializeField] private float speed = 0.5f;
    [SerializeField] private float limitMinY = -10f;

    public LayerMask whatIsGround;
    public LayerMask whatIsWater;

    public PlayerInput playerInput = null;
    private PlayerAnimation playerAnimation = null;

    private SpriteRenderer spriteRenderer = null;
    private Rigidbody2D playerRigidbody = null;

    private Vector2 velocity = Vector2.zero;
    
    public bool isJump = false;
    public bool isJumpforce = false;

    private bool isGround = false;
    private bool isWater = false;

    private void Awake()
    {
        playerInput = FindObjectOfType<PlayerInput>();
        playerAnimation = FindObjectOfType<PlayerAnimation>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (GameManager.isPause) return;

        if (playerInput.isJump)
        {
            isJump = true;
        }

        if (transform.position.y < limitMinY && !GameManager.isPause && !GameManager.Instance.isEnemyStop)
        {
            GameManager.Instance.GameEnd(eGameStates.gameOver);
        }
    }

    private void FixedUpdate()
    {
        playerRigidbody.bodyType = GameManager.isPause ? RigidbodyType2D.Static : RigidbodyType2D.Dynamic;

        if (GameManager.isPause)
        {
            return;
        }

        if (playerInput.xMove == 1f)
        {
            spriteRenderer.flipX = false;
        }
        else if (playerInput.xMove == -1f)
        {
            spriteRenderer.flipX = true;
        }

        isGround = Physics2D.OverlapCircle(groundCheckTransform.position, 0.1f, whatIsGround);
        isWater = isGround ? Physics2D.OverlapCircle(groundCheckTransform.position, 0.1f, whatIsWater) : isWater;

        if (isJump && isGround && !isJumpforce)
        {
            isJumpforce = true;
            playerRigidbody.velocity = Vector2.zero;
            playerRigidbody.AddForce(isWater ? jumpforce / 2 : jumpforce, ForceMode2D.Impulse);
            playerAnimation.JumpAnimation();
        }

        if (!isGround && playerRigidbody.velocity.y < 0)
        {
            playerAnimation.DownAnimation();
            isJump = false;
            isJumpforce = false;
        }
        else if (isJump)
        {
            isJump = false;
            isJumpforce = false;
        }
        else if (!isJump && isGround)
        {
            playerAnimation.AnimationStart();
        }

        velocity.x = playerInput.xMove * (isWater ? speed / 2 : speed);

        if (spriteRenderer.flipX)
        {
            if (Physics2D.OverlapCircle(leftGroundCheckTransform.position, 0.1f, whatIsGround))
            {
                velocity.x = 0f;
            }
        }
        else
        {
            if (Physics2D.OverlapCircle(rightGroundCheckTransform.position, 0.1f, whatIsGround))
            {
                velocity.x = 0f;
            }
        }

        velocity.y = playerRigidbody.velocity.y;
        playerRigidbody.velocity = velocity;
    }

    public void ResetMove(Vector3 position)
    {
        isJump = false;
        isJumpforce = false;

        playerRigidbody.bodyType = RigidbodyType2D.Dynamic;
        playerRigidbody.velocity = Vector2.zero;

        spriteRenderer.flipX = false;

        transform.position = position;
    }
}