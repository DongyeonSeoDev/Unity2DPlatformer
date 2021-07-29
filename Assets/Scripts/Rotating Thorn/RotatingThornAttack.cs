using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingThornAttack : MonoBehaviour
{
    private UIManager uIManager = null;

    private void Awake()
    {
        uIManager = FindObjectOfType<UIManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            uIManager.GameOver();
        }
    }
}
