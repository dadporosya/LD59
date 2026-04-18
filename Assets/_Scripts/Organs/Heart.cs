using System;
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
    }
}
