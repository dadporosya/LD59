using System;
using System.Collections;
using UnityEngine;

public class Heart : OrganBase
{
    public int BMPPerBeat = 10;
    private BPMManager bpmManager;
    public float beatPower = 1.1f;

    public float healPerBeat = 0.05f;
    
    private PlayerDamageManager  damageManager;
    
    public override void Awake()
    {
        base.Awake();
        if (!bpmManager) bpmManager = FindFirstObjectByType<BPMManager>();
        bpmManager.OnBeat.AddListener(() => StartCoroutine(BeatSize(1 + (beatPower-1)/4)));
        damageManager = FindFirstObjectByType<PlayerDamageManager>();
    }

    public override void Action()
    {
        base.Action();
        h.ShakeOnce(2f, 5f, 0, 0.2f);
        bpmManager.ChangeBPM(BMPPerBeat);
        StartCoroutine(BeatSize(beatPower));
        // damageManager.damageTaken -= healPerBeat;
        // damageManager.damageTaken = h.Max(0, damageManager.damageTaken);
    }
    
    
    
    public IEnumerator BeatSize(float scale)
    {
        yield return null;
        
        Vector3 initialScale = transform.localScale;
        yield return StartCoroutine(h.SmoothScalingCoroutine(transform, initialScale * scale, 0.1f));
        yield return StartCoroutine(h.SmoothScalingCoroutine(transform, initialScale, 0.1f));
    }
}
