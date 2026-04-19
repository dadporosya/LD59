using System;
using UnityEngine;

public class PoliceEnemy : EnemyBase,IBlindable
{
    [Header("Police Enemy")]
    private Collider2D collider2D;
    public float damage = 2.5f;

    public bool playerBumped = false;
    
    public bool blinded = false;

    private EscapeProgressManager escapeProgressManager;
    private void Awake()
    {
        collider2D = GetComponent<Collider2D>();
        escapeProgressManager = FindFirstObjectByType<EscapeProgressManager>();
    }

    // public override void Start()
    // {
    //     base.Start();
    // }

    public void OnPlayerCollision(Collider2D collision=null)
    {
        h.Out("bump");
        if (blinded) return;
        
        float speed = -5f;
        float distance = 0.2f;
        escapeProgressManager.ChangeXP(-(distance+escapeProgressManager.tempXP), distance/Mathf.Abs(speed));
        
        scrollManager.StopScroll();
        // float distance = collision != null ? Mathf.Abs(transform.position.x - collision.transform.position.x) : 0f;
        scrollManager.Scroll(distance, speed); // to polish
        
        h.ShakeOnce(2, 10, 0, 0.25f);
        
        if (!playerBumped)
        {
            h.Out("bumped 1st");

            playerBumped = true;
            return;
        }
        h.Out("damage");
        //anim
        playerDamageManager.TakeDamage((int)damage);
    }

    public void Blind(float  duration)
    {
        blinded = true;
        h.InvokeAfterTime(this, duration, () => { blinded = false; });
    }
    
}
