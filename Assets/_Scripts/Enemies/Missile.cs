using System.Collections;
using UnityEngine;

public class Missile : MonoBehaviour, IBlindable
{
    public Transform target;
    
    public float damage = 2.5f;
    public float radiusMult = 1f;
    public float reactTime=2f;
    public float missileSpeed = 10f;

    public GameObject aimPrefab;
    public GameObject explosionPrefab;

    private GameObject currentAim;

    public Transform parent;
    
    [SerializeField] private bool initOnStart = false;

    [SerializeField] public float accelerationCoof = 2f;
    
    public void Init(Transform targetIn, float damageValue, float radiusMultValue, float reactTimeValue, float missileSpeedValue, Transform parentIn=null)
    {
        target = targetIn;
        damage = damageValue;
        radiusMult = radiusMultValue;
        reactTime = reactTimeValue;
        missileSpeed = missileSpeedValue;
        
        if (parentIn) parent = parentIn;
        transform.SetParent(parent);
        
        
        Launch();
    }

    private void Start()
    {
        if (initOnStart)
        {
            Init(target, damage,radiusMult,reactTime,missileSpeed, transform);
        }
    }

    public void Launch()
    {
        currentAim= Instantiate(aimPrefab, target.position, aimPrefab.transform.rotation, parent);
        currentAim.transform.localScale = explosionPrefab.transform.localScale * radiusMult;
        
        target = currentAim.transform;
        
        StartCoroutine(LaunchCoroutine());
    }

    public IEnumerator LaunchCoroutine()
    {
        
        // Phase 1: Rotate to look at target for reactTime * 0.8
        float timeCoof = 0.98f;
        
        float rotationDuration = reactTime * timeCoof;
        float rotationElapsed = 0f;
        Vector3 startRotation = transform.eulerAngles;
        
        // Calculate target angle based on x distance / y distance
        Vector3 directionToTarget = target.position - transform.position;
        float xDistance = directionToTarget.x;
        float yDistance = directionToTarget.y;
        float targetAngle = Mathf.Atan(xDistance / yDistance) * Mathf.Rad2Deg;
        
        while (rotationElapsed < rotationDuration)
        {
            float t = rotationElapsed / rotationDuration;
            float currentAngle = Mathf.LerpAngle(startRotation.z, targetAngle, t);
            transform.eulerAngles = new Vector3(0, 0, currentAngle);
            
            rotationElapsed += Time.deltaTime;
            yield return null;
        }
        
        yield return new  WaitForSeconds(reactTime * (1-timeCoof));
        
        // Ensure we're looking directly at target
        transform.eulerAngles = new Vector3(0, 0, targetAngle);
        
        // ...existing code...
        // Phase 2: Move towards target with acceleration
        Vector3 startPosition = transform.position;
        Vector3 endPosition = target.position;
        Vector3 direction = (endPosition - startPosition).normalized;
        float distance = Vector3.Distance(startPosition, endPosition);
        
        // Calculate flight time using kinematic equation: distance = velocity * t + 0.5 * a * t^2
        float acceleration = missileSpeed * accelerationCoof;
        float initialVelocity = -2 * missileSpeed;
        
        // Solve quadratic equation: 0.5 * a * t^2 + v * t - distance = 0
        float discriminant = initialVelocity * initialVelocity + 2 * acceleration * distance;
        float flightTime = (-initialVelocity + Mathf.Sqrt(discriminant)) / acceleration;
        
        float flightElapsed = 0f;
        float velocity = initialVelocity;
        
        while (flightElapsed < flightTime)
        {
            // Update velocity with acceleration
            velocity += acceleration * Time.deltaTime;
            
            // Move in the direction based on velocity
            transform.position += direction * velocity * Time.deltaTime;
            
            flightElapsed += Time.deltaTime;
            yield return null;
        }
        
        // Ensure we reach the target position
        transform.position = endPosition;
        
        // Call Explode
        Explode();
    }

    public void Explode(bool enableCollider = true)
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity).GetComponent<Explosion>().Init(damage, radiusMult, enableCollider);
        
        if (currentAim) Destroy(currentAim);
        Destroy(gameObject);
    }

    public void Blind(float duration)
    {
        Explode(false);
    }
}
