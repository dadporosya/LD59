using System;
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

    public List<Coroutine> scrollCoroutines = new List<Coroutine>();
    
    private EscapeProgressManager  escapeProgressManager;
    private EnemiesSpawnManager enemiesSpawnManager;

    private void Start()
    {
        escapeProgressManager = FindFirstObjectByType<EscapeProgressManager>();
        enemiesSpawnManager = FindFirstObjectByType<EnemiesSpawnManager>();
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
        } else if (enemy is BarrierEnemy barrierEnemy)
        {
            h.Out("niggamoo");
            barrierEnemy.OnPlayerCollision(playerSmartCollider.collider);
        }
    }
    
    public void Scroll(float distance, float speed)
    {
        try
        {
            backgroundScroller.Scroll(distance, speed);
            if (!scrollingParent) return;
            ScrollParent(distance, speed);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public void StopScroll()
    {
        backgroundScroller.StopScroll();
        
        foreach (Coroutine coroutine in scrollCoroutines)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
        }
        scrollCoroutines.Clear();
        
        remainingDistance = 0;
        currentSpeed = 0;
        escapeProgressManager.onChangeTempXP = 0;
        escapeProgressManager.initialTempXp = escapeProgressManager.currentXP;
    }
    
    public void ScrollParent(float distance, float speed)
    {
        remainingDistance += distance;
        currentSpeed = speed;

        Coroutine newCoroutine = StartCoroutine(ScrollCoroutine());
        scrollCoroutines.Add(newCoroutine);
    }

    private IEnumerator ScrollCoroutine()
    {
        // h.Out(remainingDistance);
        while (remainingDistance > 0)
        {
            float moveAmount = currentSpeed * Time.deltaTime;
            if (moveAmount > remainingDistance) moveAmount = remainingDistance;
            
            scrollingParent.Translate(new Vector3(-moveAmount, 0f, 0f));
            
            remainingDistance -= Mathf.Abs(moveAmount);

            try
            {
                enemiesSpawnManager.CheckDeath();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            
            yield return null;
        }
        
        // Remove this coroutine from the list when it completes
        Coroutine current = null;
        foreach (Coroutine c in scrollCoroutines)
        {
            if (c != null) current = c;
        }
        if (current != null) scrollCoroutines.Remove(current);
    }

}
