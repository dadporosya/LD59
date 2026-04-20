using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float damage = 2.5f;
    public float radiusMult = 1f;

    public float fadeTime=0.8f;

    public SmartCollider smartCollider;
    public List<string> targetTags = new List<string>();

    public void Init(float damageIn, float radiusMultIn, bool explodeWithCollider=true)
    {
        damage = damageIn;
        radiusMult = radiusMultIn;
        
        if (!smartCollider) smartCollider = GetComponent<SmartCollider>();
        smartCollider.targetTags = targetTags;
        
        Explode(explodeWithCollider);
    }

    public void Explode(bool explodeWithCollider=true)
    {
        h.ShakeOnce(1.5f, 5, 0, 0.15f);
        
        if (!explodeWithCollider) smartCollider.enabled = false;
        else
        {
            smartCollider.enabled = true;
            smartCollider.onTriggerEnter.AddListener((GameObject go) =>
            {
                FindFirstObjectByType<PlayerDamageManager>().TakeDamage(damage);
            });
        }
        
        transform.localScale *= radiusMult;
        
        h.FadeOut(gameObject, fadeTime);
        h.InvokeAfterTime(this, fadeTime, () => { Destroy(gameObject); });
        
        //TODO create animator for it
    }
}
