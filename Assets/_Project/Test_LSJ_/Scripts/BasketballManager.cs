using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class BasketballManager : MonoBehaviour
{
    private float time = 0;
    private int score = 0;

    private int stage = 1;
    private bool isStageStarted = false;

    public float stageDurationTime = 60f;
    
    public TextMeshPro scoreText;
    public TextMeshPro timeText;
    public TextMeshPro stageText;

    public Transform defaultPos;
    public Transform leftPos;
    public Transform rightPos;
    public Transform backPos;
    
    public GameObject basket;
    private Coroutine horizontalCoroutine;
    private Coroutine backMoveCoroutine;
    
    private Tween horizontalTween;
    private Tween backMoveTween;

    private void Start()
    {
        defaultPos = transform;
        leftPos.SetParent(null);
        rightPos.SetParent(null);
        backPos.SetParent(null);
        scoreText.text = score.ToString();
        StartCoroutine(BetweenStageTimer());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            score += 10;
            scoreText.text =  score.ToString();
        }
    }

    public void ScoreUpdate()
    {
        if (isStageStarted == true)
        {
            if (time < 20)
            {
                score += 3;
            }
            else
            {
                score += 2;
            }
        }

        scoreText.text = score.ToString();
    }

    IEnumerator StageTimer()
    {
        stageText.text = stage.ToString();
        if (stage != 1)
        {
            if (stage == 2)
            {
                horizontalCoroutine = StartCoroutine(HorizontalMove());
            }
            else if (stage == 3)
            {
                backMoveCoroutine = StartCoroutine(BackMove());
            }
        }
        isStageStarted = true;
        time = stageDurationTime;
        while (time > 0)
        {
            time -= Time.deltaTime;
            timeText.text = string.Format("{0:0.0}", time);
            yield return null;
        }

        timeText.text = "0.0";
        if (score >= 20 * stage)
        {
            stage++;
            StartCoroutine(BetweenStageTimer());
        }

        isStageStarted = false;
        if (horizontalCoroutine != null)
        {
            StopCoroutine(horizontalCoroutine);
            horizontalTween.Kill();
            horizontalCoroutine = null;
        }

        if (backMoveCoroutine != null)
        {
            StopCoroutine(backMoveCoroutine);
            backMoveTween.Kill();
            backMoveCoroutine = null;
        }
        basket.transform.DOMove(defaultPos.position, 1f);
    }

    IEnumerator BetweenStageTimer()
    {
        time = 7f;
        while (time > 0)
        {
            time -= Time.deltaTime;
            timeText.text = string.Format("{0:0.0}", time);
            yield return null;
        }

        timeText.text = "0.0";
        StartCoroutine(StageTimer());
    }

    IEnumerator HorizontalMove()
    {
        bool needToGoLeft = true;
        bool needToGoRight = false;
        bool isMoving = false;
        float duration = 4f;
        while (true)
        {
            if (isMoving == false)
            {
                if (needToGoLeft == true)
                {
                    isMoving = true;
                    horizontalTween = basket.transform.DOMove(leftPos.position, duration).SetEase(Ease.InOutQuad)
                        .OnComplete(() =>
                        {
                            needToGoLeft = false;
                            needToGoRight = true;
                            isMoving = false;
                        });
                }
                else if (needToGoRight == true)
                {
                    isMoving = true;
                    horizontalTween = basket.transform.DOMove(rightPos.position, duration).SetEase(Ease.InOutQuad)
                        .OnComplete(() =>
                        {
                            needToGoLeft = true;
                            needToGoRight = false;
                            isMoving = false;
                        });
                }
            }

            yield return null;
        }
    }
    
    IEnumerator BackMove()
    {
        bool needToGoBack = true;
        bool needToGoFront = false;
        bool isMoving = false;
        float duration = 4f;
        while (true)
        {
            if (isMoving == false)
            {
                if (needToGoBack == true)
                {
                    isMoving = true;
                    backMoveTween = basket.transform.DOMove(backPos.position, duration).SetEase(Ease.InOutQuad)
                        .OnComplete(() =>
                        {
                            needToGoBack = false;
                            needToGoFront = true;
                            isMoving = false;
                        });
                }
                else if (needToGoFront == true)
                {
                    isMoving = true;
                    backMoveTween = basket.transform.DOMove(defaultPos.position, duration).SetEase(Ease.InOutQuad)
                        .OnComplete(() =>
                        {
                            needToGoBack = true;
                            needToGoFront = false;
                            isMoving = false;
                        });
                }
            }

            yield return null;
        }
    }
    
}