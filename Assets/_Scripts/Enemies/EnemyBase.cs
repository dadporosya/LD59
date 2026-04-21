using System;
using System.Collections;
using System.Collections.Generic;
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
        playerDamageManager = FindFirstObjectByType<PlayerDamageManager>();
        scrollManager = FindFirstObjectByType<ScrollManager>();
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
        SFXManager.Instance.PlayRandomClip(new List<string>()
        {
            "Audio/SFX/deathEnemy"
        });
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
