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

    private void Start()
    {
        currentMoveTween = transform.DOMove(nextPositions[currentPosition], 0f);
    }

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

            currentMoveTween = transform.DOMove(nextPositions[currentPosition], speeds[currentPosition])
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    isMove = false;
                });
        }
    }

    private void OnEnable()
    {
        GameManager.Instance.stageReset += EnemyReset;
    }

    private void OnDisable()
    {
        GameManager.Instance.stageReset -= EnemyReset;
    }

    private void EnemyReset()
    {
        currentMoveTween.Complete();
        currentMoveTween.Kill();

        currentPosition = 0;
        isMove = false;
        isPause = false;

        currentMoveTween = transform.DOMove(nextPositions[currentPosition], 0f);
    }
}
