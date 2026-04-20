using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLauncherEnemy : EnemyBase
{
    [Header("Missile launcher")] [SerializeField]
    private float distanceCoof = 0.8f;

    [SerializeField] private float possibleRangeCoof = 0.4f;
    
    
    public float coolDown;


    public float cameraW;

    [Header("Missiles")] public GameObject missilePrefab;

    public float damage = 2.5f;
    public float radiusMult = 1f;
    public float reactTime = 2f;
    public float missileSpeed = 10f;

    public List<GameObject> missiles;
    public int maxMissileCount = 1;

    public Coroutine targetingCoroutine;


    public bool off = false;
    public Transform target;
    public bool targetPlayer;

    public List<Transform> missileSpawnPoints;

    public GameObject aimPrefab;
    public GameObject explosionPrefab;

    public GameObject launcherBone;

    [SerializeField] private float missileTranslation = 0.1f;
    
    private void Start()
    {
        if (!off)
        {
            if (targetPlayer) target = GameObject.FindGameObjectWithTag("PlayerShadow").transform;
            StartTargeting(target);
        }
    }

    public void On()
    {
        if (!off) return;
        off = false;
        if (targetPlayer) target = GameObject.FindGameObjectWithTag("PlayerShadow").transform; // "PlayerDamageCollider"
        StartTargeting(target);
    }

    public void Off()
    {
        off = true;
        if (targetingCoroutine != null) StopCoroutine(targetingCoroutine);
    }

    public void StartTargeting(Transform targetIn)
    {
        if (!targetIn) targetIn = GameObject.FindGameObjectWithTag("PlayerDamageCollider").transform;
        cameraW = h.GetCameraWidth();
        targetingCoroutine = StartCoroutine(StartTargetingCoroutine(targetIn));
    }

    public IEnumerator StartTargetingCoroutine(Transform targetIn)
    {
        while (true)
        {
            if (Vector2.Distance(transform.position, targetIn.position) > cameraW * distanceCoof
                || missiles.Count > maxMissileCount)
            {
                yield return new WaitForEndOfFrame();
                continue;
            }

            // shoot
            Shoot(targetIn);

            yield return new WaitForSeconds(coolDown);
        }
    }

    public void Shoot(Transform targetIn)
    {
        if (playerDamageManager.aquariumParent.transform.position.x > transform.position.x) return; // temp?
        
        h.ShakeObject(this, launcherBone, 0.1f, 1f, 0.5f);
        
        Transform scrollingParent = GameObject.FindGameObjectWithTag("ScrollingParent").transform;
        Missile missile =
            Instantiate(missilePrefab, h.RandChoice(missileSpawnPoints).position, Quaternion.identity, scrollingParent)
                .GetComponent<Missile>();

        Vector3 targetPos = targetIn.position;
        targetPos.x += h.Range(-0.5f, h.GetCameraWidth() * possibleRangeCoof);

        GameObject currentAim = Instantiate(
            aimPrefab, targetPos, aimPrefab.transform.rotation, transform.parent);
        currentAim.transform.localScale = explosionPrefab.transform.localScale * radiusMult;

        missile.explosionPrefab = explosionPrefab;
        missile.aimPrefab = aimPrefab;

        missile.Init(currentAim.transform, damage, radiusMult, reactTime, missileSpeed);

        float currentTranslation = h.RangeWithCoof(missileTranslation, 0.25f);
        h.SmoothTranslating(this, missile.transform, new Vector3(-currentTranslation, -currentTranslation, 0), reactTime/5);
    }

    
}
