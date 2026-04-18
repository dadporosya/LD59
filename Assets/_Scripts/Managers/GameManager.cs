using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int bpm=90;
    [SerializeField] private TextMeshProUGUI bpmText;
    public int bpmReduction = 2;

    private Coroutine bpmCoroutine;

    private void Start()
    {
        if (!bpmText) bpmText = GameObject.Find("BPMNumberTextTMP").GetComponent<TextMeshProUGUI>();
        StartBPM();
    }
    
    public void SetBMP(int value)
    {
        bpm = value;
        Beat(true);
    }
    
    public void ChangeBMP(int delta)
    {
        bpm += delta;
        Beat(true);
    }

    public void StartBPM()
    {
        
        bpmCoroutine = StartCoroutine(BPMCoroutine());
    }

    public void StopBPM()
    {
        if (bpmCoroutine != null)
        {
            StopCoroutine(bpmCoroutine);
        }
    }

    private IEnumerator BPMCoroutine()
    {
        Beat(true);
        while (true)
        {
            Beat();
            yield return new WaitForSeconds(1);
        }
        
        yield return null;
    }

    private void Beat(bool newValue = false)
    {
        if (!newValue)
        {
            int reduction = (int)h.Range(bpmReduction * 0.5f, bpmReduction * 1.5f);
            h.Out(reduction);
            bpm -= reduction;
        }

        bpmText.text = bpm.ToString();
    }
}
