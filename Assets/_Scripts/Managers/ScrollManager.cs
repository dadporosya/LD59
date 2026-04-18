using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollManager : MonoBehaviour
{
    public BackgroundScroller backgroundScroller;
    public Transform scrollingParent;
    
    public float remainingDistance=0;
    public float currentSpeed=0;
    

    private Coroutine scrollCoroutine;

    private void Start()
    {
        if (!backgroundScroller) backgroundScroller = FindFirstObjectByType<BackgroundScroller>();
    }
    
    public void Scroll(float distance, float speed)
    {
        backgroundScroller.Scroll(distance, speed);
        if (!scrollingParent) return;
        ScrollParent(distance, speed);
        
    }
    
    public void ScrollParent(float distance, float speed)
    {
        remainingDistance += distance;
        currentSpeed = speed;

        if (scrollCoroutine != null) return;
        StartCoroutine(ScrollCoroutine());
    }

    private IEnumerator ScrollCoroutine()
    {
        while (remainingDistance > 0)
        {
            float moveAmount = currentSpeed * Time.deltaTime;
            if (moveAmount > remainingDistance) moveAmount = remainingDistance;
            
            scrollingParent.Translate(new Vector3(-moveAmount, 0f, 0f));
            
            remainingDistance -= moveAmount;
            
            yield return null;
        }
        remainingDistance = 0;
    }
}
