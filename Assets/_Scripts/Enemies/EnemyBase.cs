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
    public void TakeDamage(int damage)
    {
        damageTaken += damage;
        if (damageTaken >= durability)
        {
            Death();
        }
    }

    public void Death()
    {
        StartCoroutine(DeathCoroutine());
    }

    public IEnumerator DeathCoroutine()
    {
        yield return StartCoroutine(BeforeDestroyCoroutine());
        Destroy(gameObject);
    }

    public void Blind(float duration)
    {
        
    }

    
    public IEnumerator BeforeDestroyCoroutine()
    {
        yield return null;
    }
}
