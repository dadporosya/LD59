using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public Transform backgroundParent;
    public Queue<Transform> backgroundObjects = new Queue<Transform>();
    public float borderX;

    [SerializeField] private bool scrolling = false;
    private Coroutine scrollCoroutine;
    public float remainingDistance=0;
    public float currentSpeed=0;

    private void Start()
    {
        foreach (Transform backgroundObject in backgroundParent)
        {
            backgroundObjects.Enqueue(backgroundObject);
        }
        h.Out(backgroundObjects.Count);
        borderX = backgroundObjects.Peek().localPosition.x;
    }

    public void Scroll(float distance, float speed)
    {
        remainingDistance += distance;
        currentSpeed = speed;

        if (scrollCoroutine != null) return;
        StartCoroutine(ScrollCoroutine());
    }

    public void StopScroll()
    {
        if (scrollCoroutine != null)
        {
            StopCoroutine(scrollCoroutine);
            scrollCoroutine = null;
        }
        remainingDistance = 0;
        currentSpeed = 0;
    }

    private IEnumerator ScrollCoroutine()
    {
        while (remainingDistance > 0)
        {
            float moveAmount = currentSpeed * Time.deltaTime;
            if (moveAmount > remainingDistance) moveAmount = remainingDistance;
            
            foreach (Transform backgroundObject in backgroundObjects)
            {
                backgroundObject.Translate(new Vector3(-moveAmount, 0f, 0f));
            }
            
            remainingDistance -= moveAmount;
            
            if (backgroundObjects.Count > 0 && backgroundObjects.Peek().position.x < borderX)
            {
                Transform bg = backgroundObjects.Dequeue();
                bg.position = backgroundObjects.Peek().position + new Vector3(backgroundObjects.Peek().GetComponent<SpriteRenderer>().bounds.size.x, 0, 0);
                backgroundObjects.Enqueue(bg);
            }
            
            yield return null;
        }

        remainingDistance = 0;
    }
}