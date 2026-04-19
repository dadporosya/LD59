using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollManager : MonoBehaviour
{
    public BackgroundScroller backgroundScroller;
    public Transform scrollingParent;
    
    [HideInInspector] public float remainingDistance=0;
    [HideInInspector] public float currentSpeed=0;

    public SmartCollider playerSmartCollider;
    public List<string> collisionTags = new List<string>()
    {
        "Enemy"
    };

    private Coroutine scrollCoroutine;

    private void Start()
    {
        if (!backgroundScroller) backgroundScroller = FindFirstObjectByType<BackgroundScroller>();
        if (!playerSmartCollider) playerSmartCollider = GameObject.FindWithTag("PlayerParent").GetComponent<SmartCollider>();
        if (playerSmartCollider)
        {
            playerSmartCollider.onTriggerEnter.AddListener(
                ProcessCollision
            );
        }
        
        playerSmartCollider.targetTags.AddRange(collisionTags);
    }

    public void ProcessCollision(GameObject collision)
    {
        EnemyBase enemy = collision.GetComponent<EnemyBase>();

        if (!enemy) return;
        
        if (enemy is PoliceEnemy policeEnemy)
        {
            policeEnemy.OnPlayerCollision(playerSmartCollider.collider);
        }
    }
    
    public void Scroll(float distance, float speed)
    {
        backgroundScroller.Scroll(distance, speed);
        if (!scrollingParent) return;
        ScrollParent(distance, speed);
        
    }

    public void StopScroll()
    {
        backgroundScroller.StopScroll();
        
        if (scrollCoroutine != null)
        {
            StopCoroutine(scrollCoroutine);
            scrollCoroutine = null;
        }
        remainingDistance = 0;
        currentSpeed = 0;
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
        while (Mathf.Abs(remainingDistance) >= 0.0001f)
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
