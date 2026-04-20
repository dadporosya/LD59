using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLauncherEnemy : EnemyBase
{
    [Header("Missile launcher")] [SerializeField]
    private float distanceCoof = 0.8f;

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
        StartCoroutine(ShakeLauncherBoneCoroutine( 
            0.1f,
            1f,
            0.5f
                        
        ));
        
        Transform scrollingParent = GameObject.FindGameObjectWithTag("ScrollingParent").transform;
        Missile missile =
            Instantiate(missilePrefab, h.RandChoice(missileSpawnPoints).position, Quaternion.identity, scrollingParent)
                .GetComponent<Missile>();

        Vector3 targetPos = targetIn.position;
        targetPos.x += h.Range(0f, h.GetCameraWidth() * 0.5f);

        GameObject currentAim = Instantiate(aimPrefab, targetPos, aimPrefab.transform.rotation, transform.parent);
        currentAim.transform.localScale = explosionPrefab.transform.localScale * radiusMult;

        missile.explosionPrefab = explosionPrefab;
        missile.aimPrefab = aimPrefab;

        missile.Init(currentAim.transform, damage, radiusMult, reactTime, missileSpeed);

        h.SmoothTranslating(this, missile.transform, new Vector3(-missileTranslation, -missileTranslation, 0), reactTime/5);
    }

    public IEnumerator ShakeLauncherBoneCoroutine(
        float magnitude,
        float sharpness,
        float duration,
        float fadeInDuration=0,
        float fadeOutDuration=0
    )
    {
        if (launcherBone == null)
            yield break;

        Transform boneTransform = launcherBone.transform;

        // Save initial local position so we can restore it later
        Vector3 initialLocalPosition = boneTransform.localPosition;

        float totalDuration = fadeInDuration + duration + fadeOutDuration;
        float elapsed = 0f;

        while (elapsed < totalDuration)
        {
            elapsed += Time.deltaTime;

            float strength = 1f;

            // Fade in
            if (elapsed <= fadeInDuration)
            {
                strength = Mathf.Clamp01(elapsed / fadeInDuration);
            }
            // Sustain full shake
            else if (elapsed <= fadeInDuration + duration)
            {
                strength = 1f;
            }
            // Fade out
            else
            {
                float fadeOutTime = elapsed - fadeInDuration - duration;
                strength = 1f - Mathf.Clamp01(fadeOutTime / fadeOutDuration);
            }

            // Sharp shake offset
            Vector3 randomOffset = Random.insideUnitSphere * magnitude * strength;

            // Sharpness makes movement snappier
            randomOffset *= sharpness;

            boneTransform.localPosition = initialLocalPosition + randomOffset;

            yield return null;
        }

        // Ensure exact reset at the end
        boneTransform.localPosition = initialLocalPosition;
    }
}
