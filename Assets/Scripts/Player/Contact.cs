using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contact : MonoBehaviour
{
    [SerializeField] private eGameStates state;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player") && !GameManager.isPause && !GameManager.Instance.isEnemyStop)
        {
            GameManager.Instance.GameEnd(state);
        }
    }
}
