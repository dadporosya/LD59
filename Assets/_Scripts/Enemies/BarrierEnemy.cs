using UnityEngine;

public class BarrierEnemy : EnemyBase
{
    [Header("Barier Enemy")]
    private Collider2D collider2D;
    // public float damage = 2.5f;
    
    private EscapeProgressManager escapeProgressManager;
    private ScrollManager scrollManager;

    public float ramValuePerHit = 1.5f / 1;
    private void Awake()
    {
        collider2D = GetComponent<Collider2D>();
        escapeProgressManager = FindFirstObjectByType<EscapeProgressManager>();

        scrollManager = FindFirstObjectByType<ScrollManager>();
        
    }

    // public override void Start()
    // {
    //     base.Start();
    // }

    public void OnPlayerCollision(Collider2D collision=null)
    {
        h.Out("bump");

        float playerSpeed = scrollManager.currentSpeed;

        // int ramValue = (int)(scrollManager.currentSpeed / Preferences.defaultLegSpeed * ramValuePerHit);
        int ramValue = scrollManager.scrollCoroutines.Count;
        
        h.Out(ramValue,(scrollManager.currentSpeed / Preferences.defaultLegSpeed * ramValuePerHit));
        
        if (ramValue > 1)
        {
            TakeDamage(ramValue-1);
            // or dead insted;
            h.ShakeOnce(3, 10, 0, 0.3f);
            if (damageTaken >= durability)
            {
                return;
            }
        }
        
        
        // float distance = collision != null ? Mathf.Abs(transform.position.x - collision.transform.position.x) : 0f;
        float distance = 0.2f;
        // h.Out(distance, name, collision.gameObject.name);

        float speed = -5f;
        escapeProgressManager.ChangeXP(-(escapeProgressManager.tempXP/2), distance/Mathf.Abs(speed));
        
        scrollManager.StopScroll();
        scrollManager.Scroll(distance, speed); // to polish
        // h.Out(-(distance / Preferences.distancePerXP));
        
        
        
        h.ShakeOnce(2, 10, 0, 0.25f);
        
        // check speed
    }
}
