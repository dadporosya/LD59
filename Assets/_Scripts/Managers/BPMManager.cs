using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class BPMManager : MonoBehaviour
{
    private int startBPM = 90;
    [SerializeField] private int bpm=90;
    [SerializeField] private int fatalMinBPM = 0;
    [SerializeField] private int fatalMaxBPM = 200;
    [SerializeField] private TextMeshProUGUI bpmText;
    public float baseBPMReduction = 2;
    public float currentBPMReduction = 2;

    private Coroutine bpmCoroutine;
    private Coroutine beatCoroutine;

    [SerializeField] private float magnitude=0.5f;
    [SerializeField] private float sharpness = 7f;
    [SerializeField] private float beatDuration = 0.2f;

    private GameFlowManager gameFlowManager;
    
    public UnityEvent OnBeat;

    public void Init()
    {
        StopHeartBeat();
        
        bpm = startBPM;
        currentBPMReduction = baseBPMReduction;
        
        Start();
    }
    
    private void Start()
    {
        bpm = startBPM;
        gameFlowManager = FindFirstObjectByType<GameFlowManager>();
        currentBPMReduction = baseBPMReduction;
        if (!bpmText) bpmText = GameObject.Find("BPMNumberTextTMP").GetComponent<TextMeshProUGUI>();
        SetBPM(bpm);
        
        StartHeartBeat();
    }
    
    public void SetBPM(int value)
    {
        if (bpm == value) return;
        bpm = value;
        ProcessBeat(true);
    }

    public void ChangeBPM(int delta, bool processBeat = true)
    {
        bpm += delta;
        if (processBeat) ProcessBeat(true);
    }

    public void StartHeartBeat()
    {
        
        bpmCoroutine = StartCoroutine(BPMCoroutine());
        beatCoroutine = StartCoroutine(HeartBeatCoroutine());
    }

    public void StopHeartBeat()
    {
        // SetBPM(0);
        if (bpmCoroutine != null)
        {
            StopCoroutine(bpmCoroutine);
        }

        if (beatCoroutine != null)
        {
            StopCoroutine(beatCoroutine);
        }
        
        // ProcessBeat(true);
    }

    private IEnumerator BPMCoroutine()
    {
        ProcessBeat(true);
        while (true)
        {
            if (gameFlowManager.IsPaused())
            {
                yield return new WaitForSeconds(1);
                continue;
            }
            
            ProcessBeat();
            yield return new WaitForSeconds(1);
        }
        
        yield return null;
    }

    private IEnumerator HeartBeatCoroutine()
    {
        while (true)
        {
            if (gameFlowManager.IsPaused())
            {
                yield return new WaitForSeconds(1);
                continue;
            }
            
            HeartBeat();
            if (bpm == 0)
            {
                StopHeartBeat();
                yield break;
            }
            
            SFXManager.Instance.PlayRandomClip(new List<string>()
            {
                "Audio/SFX/heartBeat",
            }, volumeIn:0.1f);
            
            yield return new WaitForSeconds((float)60/(float)bpm);
        }
        yield return null;
    }

    private void HeartBeat()
    {
        OnBeat?.Invoke();
        h.ShakeOnce(magnitude, sharpness, 0, h.RangeWithCoof(beatDuration, 0.5f));
    }

    private void ProcessBeat(bool newValue = false)
    {
        if (!newValue)
        {
            int reduction = -1 * (int)h.RangeWithCoof(currentBPMReduction, 0.2f);
            bpm += reduction;
        }
        
        // h.Out(bpm);
        bpmText.text = bpm.ToString();
        
        if (gameFlowManager.state == GameFlowManager.States.Finale) return;
        
        if (bpm <= fatalMinBPM)
        {
            StopAllCoroutines();
            StopHeartBeat();
            gameFlowManager.currentDeathMessage = gameFlowManager.heartStopDeathMessage;
            gameFlowManager.SetOnLoss();
            
        } else if (bpm >= fatalMaxBPM)
        {
            StopAllCoroutines();
            StopHeartBeat();
            h.Out("tooFar");
            gameFlowManager.currentDeathMessage = gameFlowManager.tooFarPushHeartDeathMessage;
            gameFlowManager.SetOnLoss();
            
        }
    }
    
}
