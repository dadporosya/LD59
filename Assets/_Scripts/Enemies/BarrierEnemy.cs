using System.Collections.Generic;
using UnityEngine;

public class BarrierEnemy : EnemyBase
{
    [Header("Barier Enemy")]
    private Collider2D collider2D;
    // public float damage = 2.5f;
    
    private EscapeProgressManager escapeProgressManager;
    private ScrollManager scrollManager;

    public float ramValuePerHit = 1.5f / 1;
    
    public List<GameObject> cracks = new List<GameObject>();
    [SerializeField] private List<Transform> crackSpawnPoints= new List<Transform>();
    private void Awake()
    {
        collider2D = GetComponent<Collider2D>();
        escapeProgressManager = FindFirstObjectByType<EscapeProgressManager>();

        scrollManager = FindFirstObjectByType<ScrollManager>();
        
        // Find all children with tag "Point"
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Point"))
            {
                crackSpawnPoints.Add(child);
            }
        }
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

        float addionalXPReduction = -5f;
        
        if (ramValue > 1)
        {
            addionalXPReduction = 0;
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
        // escapeProgressManager.SetXP(escapeProgressManager.initialTempXp - distance / Preferences.distancePerXP, distance/Mathf.Abs(speed));

        h.Out((distance / Preferences.distancePerXP
                    + (scrollManager.scrollCoroutines.Count * Preferences.defaultLegSpeed -
                       escapeProgressManager.onChangeTempXP)),
            scrollManager.scrollCoroutines.Count * Preferences.defaultLegSpeed,
            escapeProgressManager.onChangeTempXP);
        
        escapeProgressManager.ChangeXP(
            (distance/Preferences.distancePerXP
              + (scrollManager.scrollCoroutines.Count * Preferences.defaultLegSpeed - escapeProgressManager.onChangeTempXP) + addionalXPReduction),
            distance/Mathf.Abs(speed));
        
        
        scrollManager.StopScroll();
        scrollManager.Scroll(distance, speed); // to polish
        // h.Out(-(distance / Preferences.distancePerXP));
        //
        
        
        h.ShakeOnce(2, 10, 0, 0.25f);
        
        // check speed
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        
        // Instantiate random crack above the sprite
        if (cracks.Count == 0) return;
        
        for (int i = 0; i<damage; i++){
            GameObject crack = Instantiate(h.RandChoice(cracks), transform);
            
            // // Position above the sprite
            // if (TryGetComponent<SpriteRenderer>(out SpriteRenderer spriteRenderer))
            // {
            //     float spriteHeight = spriteRenderer.sprite.bounds.size.y * transform.localScale.y;
            //     crack.transform.localPosition = new Vector3(0, spriteHeight / 2, 0);
            // }
            float crakMultScale = 0.7f;
            crack.transform.localScale = new Vector3(h.RangeWithCoof(crakMultScale, 0.2f), h.RangeWithCoof(crakMultScale, 0.2f), 0.5f);
            crack.transform.position = crackSpawnPoints.Count == 0
                ? transform.position
                : h.RandChoice(crackSpawnPoints).position;
        }
    }
}
