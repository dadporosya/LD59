using System;
using System.Collections;
using UnityEngine;

public class EnemyBase : MonoBehaviour, IOnDestroy
{
    public bool immortal=false;
    public int durability = 1;
    public int damageTaken = 0;

    public PlayerDamageManager playerDamageManager;
    public ScrollManager scrollManager;
    
    public virtual void Start()
    {
        if (!playerDamageManager) playerDamageManager = FindFirstObjectByType<PlayerDamageManager>();
        if (!scrollManager) scrollManager = FindFirstObjectByType<ScrollManager>();
    }
    public virtual void TakeDamage(int damage)
    {
        damageTaken += damage;
        if (damageTaken >= durability)
        {
            Death();
        }
    }

    public void OnDestroy()
    {
        Death();
    }

    public void Death()
    {
        // try
        // {
            StartCoroutine(DeathCoroutine());
        // }
        // catch (Exception e)
        // {
        //     Console.WriteLine(e);
        //     throw;
        // }
        
    }

    public IEnumerator DeathCoroutine()
    {
        yield return StartCoroutine(BeforeDestroyCoroutine());
        Destroy(gameObject);
    }

    
    public IEnumerator BeforeDestroyCoroutine()
    {
        yield return null;
    }
}
