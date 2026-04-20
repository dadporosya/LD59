using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class IntroCutscene : CutSceneBase
{
    [SerializeField] private bool intro = false;
    [SerializeField] private float waitDuration=0.2f;
    [SerializeField] private float fadeDuration=2f;
    [SerializeField] private float introDuration=5f;
    
    
    
    private GameObject introUI;
    public override void Init()
    {
        base.Init();
        introUI = GameObject.Find("IntroUI");
        List<IEnumerator> rawSteps = new List<IEnumerator>();

        if (!intro)
        {
            waitDuration=0f;
            fadeDuration=0f;
            introDuration=0f;
        }

        rawSteps = new List<IEnumerator>()
        {
            FadeIn(0),
            Wait(waitDuration),
            Intro(),
            FadeIn(fadeDuration),
            RemoveIntroUI(),
            Wait(waitDuration),
            FadeOut(fadeDuration),
            BeginGame(),
        };
        
        
        
        

        foreach (IEnumerator step in rawSteps)
        {
            cutsceneSteps.Add(step);
        }
    }

    public IEnumerator Intro()
    {
        StartCoroutine(FadeIn(0));
        introUI.SetActive(true);
        introUI.SetActive(true);
        StartCoroutine(FadeOut(fadeDuration));
        float currentTime=0;
        while (currentTime < introDuration)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // h.ShakeOnce(1, 1, 0, 0.1f);
                break;
            }
            currentTime += Time.deltaTime;
            yield return null;
        }
        yield return null;
    }

    public IEnumerator RemoveIntroUI()
    {
        Destroy(introUI);
        yield return null;
    }

    public IEnumerator BeginGame()
    {
        GameFlowManager gameFlowManager = FindFirstObjectByType<GameFlowManager>();
        DialogueManager dialogueManager = FindFirstObjectByType<DialogueManager>();
        
        yield return new WaitForSeconds(0.5f);
        
        gameFlowManager.SetOnPause();
        dialogueManager.GetComponent<Talkable>().Talk(0);

        void Action()
        {
            gameFlowManager.SetOnGame();
            dialogueManager.onDialogueEnd.RemoveListener(Action);
        }
        
        dialogueManager.onDialogueEnd.AddListener(Action);
    }
}
