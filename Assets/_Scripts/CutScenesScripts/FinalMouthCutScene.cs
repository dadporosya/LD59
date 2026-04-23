using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class FinalMouthCutscene : CutSceneBase
{
    [SerializeField] float gapBetweenWords = 1f;
    [SerializeField] private Mouth mouthPrefab;
    [SerializeField] private Mouth mouth;
    private GameObject finalCanvas;
    [SerializeField] private float pauseBeforeCutscene = 0f;
    [SerializeField] private float translateDuration=3f;
    
    [SerializeField] private float talkShakeMagnitude=7f;
    public override void Init()
    {
        base.Init();
        List<IEnumerator> rawSteps = new List<IEnumerator>()
        {
            TranslateMouthToCenter(translateDuration),
            CutsceneStep(),
            CloseGame(),
        };
        
        foreach (IEnumerator step in rawSteps)
        {
            cutsceneSteps.Add(step);
        }
    }

    public IEnumerator TranslateMouthToCenter(float duration = 1f)
    {
        FindFirstObjectByType<GameFlowManager>().SetOnPause();
        yield return new WaitForSeconds(pauseBeforeCutscene);
        
        GameObject.FindGameObjectWithTag("UICanvas").SetActive(false);

        MusicManager.Instance.ShutdownMusic(duration);
        
        mouth = FindFirstObjectByType<Mouth>();
        if (!mouth) mouth = Instantiate(mouthPrefab);
        
        finalCanvas =  GameObject.FindGameObjectWithTag("FinalCanvas");
        Image image =  finalCanvas.GetComponentInChildren<Image>();
        
        // Target values
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 10f);
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(screenCenter);
        Vector3 targetScale = Vector3.one;
        
        // Starting values
        Vector3 startPosition = mouth.transform.position;
        Vector3 startScale = mouth.transform.localScale;
        float startImageAlpha = image.color.a;
        
        // Smoothly animate to target
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            
            mouth.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            mouth.transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            
            // Smoothly change image alpha to 1
            Color imageColor = image.color;
            imageColor.a = Mathf.Lerp(startImageAlpha, 1f, t);
            image.color = imageColor;
            
            yield return null;
        }
        
        // Ensure final values are exact
        mouth.transform.position = targetPosition;
        mouth.transform.localScale = targetScale;
        Color finalImageColor = image.color;
        finalImageColor.a = 1f;
        image.color = finalImageColor;
        
        yield return new  WaitForSeconds(2f);
    }
    public IEnumerator CutsceneStep()
    {
        List<string> phrase = new List<string>()
        {
            "Thanks", " For", " Playing!"
        };
        if (!mouth) mouth = FindFirstObjectByType<Mouth>();
        if (!finalCanvas) finalCanvas =  GameObject.FindGameObjectWithTag("FinalCanvas");
        TextMeshProUGUI text = finalCanvas.GetComponentInChildren<TextMeshProUGUI>();
        text.text = "";

        gapBetweenWords = h.Max(mouth.gapBetweenFrames, gapBetweenWords);

        foreach (var word in phrase)
        {
            text.text += word;
            mouth.Talk();
            h.ShakeOnce(talkShakeMagnitude, 2, 0, gapBetweenWords/2);
            yield return new WaitForSeconds(gapBetweenWords);
        }
        
        yield return new  WaitForSeconds(10f);
        
        
    }

    public IEnumerator CloseGame()
    {
        yield return null;
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
