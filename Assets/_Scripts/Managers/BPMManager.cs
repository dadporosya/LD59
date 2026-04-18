using System.Collections;
using TMPro;
using UnityEngine;

public class BPMManager : MonoBehaviour
{
    [SerializeField] private int bpm=90;
    [SerializeField] private TextMeshProUGUI bpmText;
    public int bpmReduction = 2;

    private Coroutine bpmCoroutine;
    private Coroutine beatCoroutine;

    [SerializeField] private float magnitude=0.5f;
    [SerializeField] private float sharpness = 7f;
    [SerializeField] private float beatDuration = 0.2f;

    private void Start()
    {
        if (!bpmText) bpmText = GameObject.Find("BPMNumberTextTMP").GetComponent<TextMeshProUGUI>();
        StartHeartBeat();
    }
    
    public void SetBMP(int value)
    {
        bpm = value;
        ProcessBeat(true);
    }
    
    public void ChangeBMP(int delta)
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
        h.ShakeOnce(magnitude, sharpness, 0, h.RangeWithCoof(beatDuration, 0.5f));
    }

    private void ProcessBeat(bool newValue = false)
    {
        if (!newValue)
        {
            int reduction = (int)h.RangeWithCoof(bpmReduction, 0.5f);
            bpm -= reduction;
        }

        bpmText.text = bpm.ToString();
    }
}
