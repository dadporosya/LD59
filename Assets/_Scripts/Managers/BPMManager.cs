using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class BPMManager : MonoBehaviour
{
    [SerializeField] private int bpm=90;
    [SerializeField] private int fatalBPM = 0;
    [SerializeField] private TextMeshProUGUI bpmText;
    public float baseBPMReduction = 2;
    public float currentBPMReduction = 2;

    private Coroutine bpmCoroutine;
    private Coroutine beatCoroutine;

    [SerializeField] private float magnitude=0.5f;
    [SerializeField] private float sharpness = 7f;
    [SerializeField] private float beatDuration = 0.2f;

    public UnityEvent OnBeat;

    public void Init()
    {
        bpm = 90;
        currentBPMReduction = baseBPMReduction;
        
        if (bpmCoroutine != null) StopCoroutine(bpmCoroutine);
        if (beatCoroutine != null) StopCoroutine(beatCoroutine);
        
        Start();
    }
    
    private void Start()
    {
        currentBPMReduction = baseBPMReduction;
        if (!bpmText) bpmText = GameObject.Find("BPMNumberTextTMP").GetComponent<TextMeshProUGUI>();
        SetBPM(bpm);
        
        StartHeartBeat();
    }
    
    public void SetBPM(int value)
    {
        bpm = value;
        ProcessBeat(true);
    }
    
    public void ChangeBPM(int delta)
    {
        bpm += delta;
        ProcessBeat(true);
    }

    public void StartHeartBeat()
    {
        
        bpmCoroutine = StartCoroutine(BPMCoroutine());
        beatCoroutine = StartCoroutine(HeartBeatCoroutine());
    }

    public void StopHeartBeat()
    {
        SetBPM(0);
        h.Out("stop heart");
        if (bpmCoroutine != null)
        {
            StopCoroutine(bpmCoroutine);
        }

        if (beatCoroutine != null)
        {
            StopCoroutine(beatCoroutine);
        }
    }

    private IEnumerator BPMCoroutine()
    {
        ProcessBeat(true);
        while (true)
        {
            ProcessBeat();
            if (bpm <= fatalBPM)
            {
                StopHeartBeat();
            }
            yield return new WaitForSeconds(1);
        }
        
        yield return null;
    }

    private IEnumerator HeartBeatCoroutine()
    {
        while (true)
        {
            HeartBeat();
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
            int reduction = -1 * (int)h.RangeWithCoof(currentBPMReduction, 0.5f);
            ChangeBPM(reduction);
        }
        
        bpmText.text = bpm.ToString();
    }
    
}
