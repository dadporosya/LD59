using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    [SerializeField] private Collider2D movementArea; // Green collider (allowed area)
    [SerializeField] private float floatingSpeed = 2f;
    [SerializeField] private float ignoreDuration = 0.1f;

    private Collider2D objectCollider;
    private Vector2 direction;
    private float ignoreTimer = 0f;

    private void Start()
    {
        objectCollider = GetComponent<Collider2D>();

        // Random start direction
        direction = Random.insideUnitCircle.normalized;
    }

    private void Update()
    {
        // Move object
        transform.Translate(direction * floatingSpeed * Time.deltaTime);

        // If object's collider is no longer fully inside green area -> bounce
        if (!IsFullyInside() && ignoreTimer <= 0)
        {
            Bounce();
        }

        // Handle ignore timer
        if (ignoreTimer > 0)
        {
            ignoreTimer -= Time.deltaTime;
        }
    }

    private bool IsFullyInside()
    {
        Bounds areaBounds = movementArea.bounds;
        Bounds objBounds = objectCollider.bounds;

        // Check if all sides of black collider are inside green collider bounds
        return areaBounds.Contains(objBounds.min) &&
               areaBounds.Contains(objBounds.max);
    }

    private void Bounce()
    {
        // Reverse direction
        direction = -direction;

        // Small randomness for more natural movement
        direction += Random.insideUnitCircle * 0.25f;
        direction.Normalize();

        // Push object slightly back inside to prevent sticking
        transform.Translate(direction * floatingSpeed * Time.deltaTime * 2f);

        // Reset ignore timer
        ignoreTimer = ignoreDuration;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // TODO fix collision with others floating objects
        if (collision.CompareTag("FloatingObject"))
        {
            Bounce();
        }
        // Ignore collisions with the edges immediately after bouncing
        if (ignoreTimer > 0)
        {
            Physics2D.IgnoreCollision(objectCollider, collision, true);
            return;
        }

        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Restore collision detection when exiting the ignore period
        if (collision == movementArea && ignoreTimer <= 0)
        {
            Physics2D.IgnoreCollision(objectCollider, collision, false);
        }
    }
}