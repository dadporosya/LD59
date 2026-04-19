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

        int ramValue = (int)(scrollManager.currentSpeed / Preferences.defaultLegSpeed * ramValuePerHit);

        if (ramValue > 0)
        {
            TakeDamage(ramValue);
            h.ShakeOnce(3, 10, 0, 0.3f);
            if (damageTaken >= durability)
            {
                return;
            }
        }
        
        scrollManager.StopScroll();
        // float distance = collision != null ? Mathf.Abs(transform.position.x - collision.transform.position.x) : 0f;
        float distance = 0.2f;
        h.Out(distance, name, collision.gameObject.name);

        float speed = -5f;
        scrollManager.Scroll(distance, speed); // to polish
        escapeProgressManager.ChangeXP(-(distance / Preferences.distancePerXP), distance/Mathf.Abs(speed));
        
        h.ShakeOnce(2, 10, 0, 0.25f);
        
        // check speed
    }
}
