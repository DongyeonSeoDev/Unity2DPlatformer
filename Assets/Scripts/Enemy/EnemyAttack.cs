using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player") && !GameManager.isPause && !GameManager.Instance.isEnemyStop)
        {
            GameManager.GameOver();
        }
    }
}
