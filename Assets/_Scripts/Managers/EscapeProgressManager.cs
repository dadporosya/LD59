using UnityEngine;

public class EscapeProgressManager : MonoBehaviour
{
    public int currentXP=0;
    public int maxXP=100;
    
    public Bar progressBar;

    private void Start()
    {
        if (!progressBar) progressBar = GameObject.Find("EscapeProgressBar").GetComponent<Bar>();
        UpdateBar();
    }

    public void UpdateBar()
    {
        progressBar.Init(currentXP, maxXP);
    }

    public void SetXP(int value)
    {
        currentXP = value;
        UpdateBar();
        CheckOverflow();
    }

    public void ChangeXP(int value)
    {
        currentXP += value;
        UpdateBar();
        CheckOverflow();
    }

    public void CheckOverflow()
    {
        // new organ or smt else
        if (currentXP < maxXP) return;
        
        // win cond
        
    }

}
