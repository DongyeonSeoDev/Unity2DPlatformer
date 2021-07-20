using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float speed = 0.5f;

    private PlayerInput playerInput = null;
    private SpriteRenderer spriteRenderer = null;
    private Rigidbody2D playerRigidbody = null;

    private Vector2 velocity = Vector2.zero;

    private void Awake()
    {
        playerInput = FindObjectOfType<PlayerInput>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (playerInput.xMove == 1f)
        {
            spriteRenderer.flipX = false;
        }
        else if (playerInput.xMove == -1f)
        {
            spriteRenderer.flipX = true;
        }

        velocity.x = playerInput.xMove * speed;
        velocity.y = playerRigidbody.velocity.y;

        playerRigidbody.velocity = velocity;
    }
}
