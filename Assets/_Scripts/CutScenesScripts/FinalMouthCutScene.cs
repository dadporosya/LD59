using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class FinalMouthCutscene : CutSceneBase
{
    [SerializeField] float gapBetweenWords = 0.4f;
    public override void Init()
    {
        base.Init();
        List<IEnumerator> rawSteps = new List<IEnumerator>()
        {
            CutsceneStep(),
        };
        
        foreach (IEnumerator step in rawSteps)
        {
            cutsceneSteps.Add(step);
        }
    }

    public IEnumerator CutsceneStep()
    {
        Mouth mouth = FindFirstObjectByType<Mouth>();
        
        
        
        yield return null;
    }

    
}
