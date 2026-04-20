using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class LossCutscene : CutSceneBase
{

    public override void Init()
    {
        base.Init();

        List<IEnumerator> rawSteps = new List<IEnumerator>()
        {
            SetOnLossCoroutine(),
        };
        

        foreach (IEnumerator step in rawSteps)
        {
            cutsceneSteps.Add(step);
        }
    }
    
    private IEnumerator SetOnLossCoroutine()
    {
        GameFlowManager gameFlowManager = FindFirstObjectByType<GameFlowManager>();
        
        // Zoom camera to PlayerParent
        Vector3 targetCameraPos = gameFlowManager.playerParent.transform.position;
        targetCameraPos.z = Camera.main.transform.position.z; // Keep the same Z
        
        float zoomDuration = 0.5f;
        float elapsed = 0f;
        Vector3 startCameraPos = Camera.main.transform.position;
        
        while (elapsed < zoomDuration)
        {
            elapsed += Time.deltaTime;
            Camera.main.transform.position = Vector3.Lerp(startCameraPos, targetCameraPos, elapsed / zoomDuration);
            yield return null;
        }
        
        Camera.main.transform.position = targetCameraPos;


        yield return new WaitForSeconds(0.5f);
        // Then process loss
        gameFlowManager.ProcessLoss();
    }
}
