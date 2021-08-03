using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] private Vector2[] nextPositions;
    [SerializeField] private float[] speeds;

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

        if (isPause && !GameManager.isPause)
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
