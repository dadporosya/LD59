using UnityEngine;

public class BarierEnemy : EnemyBase
{
    [Header("Barier Enemy")]
    private Collider2D collider2D;
    public float damage = 2.5f;
    
    private EscapeProgressManager escapeProgressManager;
    private ScrollManager scrollManager;
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
