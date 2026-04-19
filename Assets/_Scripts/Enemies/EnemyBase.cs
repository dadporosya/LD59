using System.Collections;
using UnityEngine;

public class EnemyBase : MonoBehaviour, IOnDestroy
{
    public int durability = 1;
    public int damageTaken = 0;

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

    public void Blind()
    {
        
    }

    
    public IEnumerator BeforeDestroyCoroutine()
    {
        yield return null;
    }
}
