using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] private Vector2[] nextPositions = null;
    [SerializeField] private float[] speeds = null;

    private bool isMove = false;
    private bool isPause = false;
    private int currentPosition = 0;

    private Tween currentMoveTween = null;

    private void Update()
    {
        if (GameManager.isPause)
        {
            currentMoveTween.Pause();
            isPause = true;
        }

        if (GameManager.Instance.isEnemyStop)
        {
            currentMoveTween.Kill();
        }

        if (isPause && !GameManager.isPause && !GameManager.Instance.isEnemyStop)
        {
            currentMoveTween.Play();
            isPause = false;
        }

        if (!isMove)
        {
            currentPosition = (currentPosition + 1) % nextPositions.Length;
            isMove = true;

            currentMoveTween = transform.DOMove(nextPositions[currentPosition], speeds[currentPosition]).SetEase(Ease.Linear).OnComplete(() =>
            {
                isMove = false;
            });
        }
    }
}
