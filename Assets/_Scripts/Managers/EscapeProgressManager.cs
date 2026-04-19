using UnityEngine;

public class EscapeProgressManager : MonoBehaviour
{
    public float currentXP=0;
    public float maxXP=100;
    
    public Bar progressBar;
    
    public float tempXP=0;

    private void Start()
    {
        if (!progressBar) progressBar = GameObject.Find("EscapeProgressBar").GetComponent<Bar>();
        UpdateBar();
    }

    public void UpdateBar()
    {
        progressBar.Init(currentXP, maxXP);
    }

    public void SetXP(float value)
    {
        currentXP = value;
        UpdateBar();
        CheckOverflow();
    }

    public void ChangeXP(float value, float newTimeToFadeFill = -1f)
    {
        currentXP += value;
        if (newTimeToFadeFill >= 0f)
        {
            ChangeBarFillTime(newTimeToFadeFill);
        }
        UpdateBar();
        CheckOverflow();
    }

    public void CheckOverflow()
    {
        // new organ or smt else
        if (currentXP < maxXP) return;
        
        // win cond
        
    }

    public void ChangeBarFillTime(float duration)
    {
        progressBar.drainDuration = duration;
    }

}
