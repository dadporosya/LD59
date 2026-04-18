using System;
using System.Collections;
using UnityEngine;

public class Heart : OrganBase
{
    public int BMPPerBeat = 10;
    private BPMManager bpmManager;
    
    public override void Awake()
    {
        base.Awake();
        if (!bpmManager) bpmManager = FindFirstObjectByType<BPMManager>();
    }

    public override void Action()
    {
        base.Action();
        h.ShakeOnce(2f, 5f, 0, 0.2f);
        bpmManager.ChangeBMP(BMPPerBeat);
        StartCoroutine(BeatSize());
    }

    public IEnumerator BeatSize()
    {
        yield return null;
        
        Vector3 initialScale = transform.localScale;
        yield return StartCoroutine(h.SmoothScalingCoroutine(transform, initialScale * 1.1f, 0.1f));
        yield return StartCoroutine(h.SmoothScalingCoroutine(transform, initialScale, 0.1f));
    }
}
