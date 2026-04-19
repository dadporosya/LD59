using UnityEngine;

public class EscapeProgressManager : MonoBehaviour
{
    public float currentXP=0;
    public float maxXP=100;
    
    public Bar progressBar;
    
    public float onChangeTempXP=0;
    public float initialTempXp=float.MaxValue;

    public EnemiesSpawnManager enemiesSpawnManager;

    private void Start()
    {
        if (!progressBar) progressBar = GameObject.Find("EscapeProgressBar").GetComponent<Bar>();
        enemiesSpawnManager = FindFirstObjectByType<EnemiesSpawnManager>();
        UpdateBar();
    }

    public void UpdateBar()
    {
        progressBar.Init(currentXP, maxXP);
    }

    public void SetXP(float value, float newTimeToFadeFill = -1f)
    {
        enemiesSpawnManager.ProcessXP(value - currentXP);
        currentXP = value;
        if (newTimeToFadeFill >= 0f)
        {
            ChangeBarFillTime(newTimeToFadeFill);
        }
        UpdateBar();
        CheckOverflow();
    }

    public void ChangeXP(float value, float newTimeToFadeFill = -1f)
    {
        enemiesSpawnManager.ProcessXP(value);
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
