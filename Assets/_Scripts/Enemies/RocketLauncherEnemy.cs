using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncherEnemy : EnemyBase
{
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

    public void StartTargeting(Transform target)
    {
        cameraW = h.GetCameraWidth();
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
            
            yield return  new WaitForSeconds(coolDown);
        }
    }

    public void Shoot(Transform target)
    {
        
    }
}
