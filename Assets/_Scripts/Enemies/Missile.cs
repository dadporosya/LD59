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
    
    [SerializeField] private bool initOnStart = false;
    
    public void Init(Transform targetIn, float damageValue, float radiusMultValue, float reactTimeValue, float missileSpeedValue)
    {
        target = targetIn;
        damage = damageValue;
        radiusMult = radiusMultValue;
        reactTime = reactTimeValue;
        missileSpeed = missileSpeedValue;
        
        Launch();
    }

    private void Start()
    {
        if (initOnStart)
        {
            Init(target, damage,radiusMult,reactTime,missileSpeed);
        }
    }

    public void Launch()
    {
        StartCoroutine(LaunchCoroutine());
    }

    public IEnumerator LaunchCoroutine()
    {
        if (aimPrefab)
        {
            currentAim= Instantiate(aimPrefab, target.position, Quaternion.identity);
            currentAim.transform.localScale = explosionPrefab.transform.localScale * radiusMult;
        }
        
        // Phase 1: Rotate to look at target for reactTime
        float rotationElapsed = 0f;
        Vector3 startRotation = transform.eulerAngles;
        
        while (rotationElapsed < reactTime)
        {
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            float targetAngle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
            
            float t = rotationElapsed / reactTime;
            float currentAngle = Mathf.LerpAngle(startRotation.z, targetAngle, t);
            transform.eulerAngles = new Vector3(0, 0, currentAngle);
            
            rotationElapsed += Time.deltaTime;
            yield return null;
        }
        
        // Ensure we're looking directly at target
        Vector3 finalDirection = (target.position - transform.position).normalized;
        float finalAngle = Mathf.Atan2(finalDirection.y, finalDirection.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, finalAngle);
        
        // Phase 2: Move towards target with parabolic trajectory
        Vector3 startPosition = transform.position;
        Vector3 endPosition = target.position;
        float distance = Vector3.Distance(startPosition, endPosition);
        float travelTime = distance / missileSpeed;
        
        float travelElapsed = 0f;
        while (travelElapsed < travelTime)
        {
            float progress = travelElapsed / travelTime;
            
            // Linear horizontal movement
            Vector3 horizontalPos = Vector3.Lerp(startPosition, endPosition, progress);
            
            // Parabolic vertical movement (quad curve)
            float parabolicHeight = Mathf.Sin(progress * Mathf.PI) * distance * 0.3f;
            
            transform.position = horizontalPos + Vector3.up * parabolicHeight;
            
            travelElapsed += Time.deltaTime;
            yield return null;
        }
        
        // Ensure we reach the target position
        transform.position = endPosition;
        
        // Call Explode
        Explode();
    }

    public void Explode(bool enableCollider = true)
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        
        if (currentAim) Destroy(currentAim);
        Destroy(gameObject);
    }

    public void Blind()
    {
        Explode(false);
    }
}
