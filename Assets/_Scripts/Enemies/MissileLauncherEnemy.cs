using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLauncherEnemy : EnemyBase
{
    [Header("Missile launcher")]
    [SerializeField] private float distanceCoof=0.8f;
    public float coolDown;
    

    public float cameraW;

    [Header("Missiles")]
    public GameObject missilePrefab;

    public float damage = 2.5f;
    public float radiusMult = 1f;
    public float reactTime=2f;
    public float missileSpeed = 10f;
    
    public List<GameObject> missiles;
    public int maxMissileCount = 1;

    public Coroutine targetingCoroutine;


    public bool off = false;
    public Transform target;
    public bool targetPlayer;

    private void Start()
    {
        if (!off)
        {
            if (targetPlayer) target = GameObject.FindGameObjectWithTag("PlayerDamageCollider").transform;
            StartTargeting(target);
        }
    }

    public void On()
    {
        if (!off) return;
        off = false;
        if (targetPlayer) target = GameObject.FindGameObjectWithTag("PlayerDamageCollider").transform;
        StartTargeting(target);
    }

    public void Off()
    {
        off = true;
        if (targetingCoroutine != null) StopCoroutine(targetingCoroutine);
    }
    
    public void StartTargeting(Transform target)
    {
        if (!target) target = GameObject.FindGameObjectWithTag("PlayerDamageCollider").transform;
        cameraW = h.GetCameraWidth();
        targetingCoroutine = StartCoroutine(StartTargetingCoroutine(target));
    }
    public IEnumerator StartTargetingCoroutine(Transform target)
    {
        while (true)
        {
            if (Vector2.Distance(transform.position, target.position) > cameraW * distanceCoof
                || missiles.Count > maxMissileCount)
            {
                yield return new WaitForEndOfFrame();
                continue;
            }
            
            // shoot
            Shoot(target);
            
            yield return  new WaitForSeconds(coolDown);
        }
    }

    public void Shoot(Transform target)
    {
        Transform scrollingParent = GameObject.FindGameObjectWithTag("ScrollingParent").transform;
        Missile missile = Instantiate(missilePrefab, scrollingParent).GetComponent<Missile>();
        
        missile.Init(target, damage, radiusMult, reactTime, missileSpeed, scrollingParent);
    }
}
