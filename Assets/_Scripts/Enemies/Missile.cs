using System.Collections;
using UnityEngine;

public class Missile : MonoBehaviour, IBlindable
{
    public Transform target;
    
    public float damage = 2.5f;
    public float radiusMult = 1f;
    public float reactTime=2f;
    public float reactTimeCoof = 1f;
    public float missileSpeed = 10f;

    public GameObject aimPrefab;
    public GameObject explosionPrefab;

    private GameObject currentAim;

    public Transform parent;
    
    [SerializeField] private bool initOnStart = false;

    [SerializeField] public float accelerationCoof = 2f;
    
    [SerializeField] private bool _blinded = false;

    public bool blinded
    {
        get { return _blinded; }
        set { _blinded = value; }
    }
    public void Init(Transform targetIn, float damageValue, float radiusMultValue, float reactTimeValue, float missileSpeedValue, Transform parentIn=null)
    {
        target = targetIn;
        damage = damageValue;
        radiusMult = radiusMultValue;
        reactTime = reactTimeValue;
        missileSpeed = missileSpeedValue;

        parent = GameObject.FindGameObjectWithTag("ScrollingParent").transform;
        transform.SetParent(parent);
        
        
        Launch();
    }

    private void Start()
    {
        parent = GameObject.FindGameObjectWithTag("ScrollingParent").transform;
        h.Out(parent);
        if (initOnStart)
        {
            Init(target, damage,radiusMult,reactTime,missileSpeed, transform);
        }
    }

    public void Launch()
    {
        StartCoroutine(LaunchCoroutine());
    }

    public IEnumerator LaunchCoroutine()
    {
        float startAngleZ = transform.eulerAngles.z;
        float targetAngleZ = 0f;

        if (target != null)
        {
            Vector3 dir = target.position - transform.position;
            targetAngleZ = -1* Mathf.Atan2(Mathf.Abs(dir.x), Mathf.Abs(dir.y)) * Mathf.Rad2Deg + 45*Mathf.Rad2Deg;
        }
        h.Out(targetAngleZ);

        float elapsed = 0f;
        while (elapsed < reactTime * reactTimeCoof)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / reactTime;
        
            float angleZ = Mathf.LerpAngle(startAngleZ, targetAngleZ, t);
            transform.rotation = Quaternion.Euler(0f, 0f, angleZ);

            yield return null;
        }

        transform.rotation = Quaternion.Euler(0f, 0f, targetAngleZ);

        yield return new WaitForSeconds(reactTime * (1-reactTimeCoof));

        // --- Phase 2: Accelerate toward target and explode on arrival ---
        float velocity = -2f * missileSpeed; // starts negative (pulls back before launching)
        float acceleration = missileSpeed * accelerationCoof;

        while (true)
        {
            if (target == null)
            {
                Explode();
                yield break;
            }

            Vector3 direction = (target.position - transform.position).normalized;
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            velocity += acceleration * Time.deltaTime;
            float step = velocity * Time.deltaTime;

            // Arrived at or overshot target
            if (velocity > 0f && step >= distanceToTarget)
            {
                transform.position = target.position;
                break;
            }

            transform.position += direction * step;

            yield return null;
        }

        Explode();
    }

    public void Explode(bool enableCollider = true)
    {
        Instantiate(explosionPrefab, target.position, Quaternion.identity, parent).GetComponent<Explosion>().Init(damage, radiusMult, enableCollider);
        
        if (target) Destroy(target.gameObject);
        Destroy(gameObject);
    }

    public void Blind(float duration)
    {
        Explode(false);
    }
}
