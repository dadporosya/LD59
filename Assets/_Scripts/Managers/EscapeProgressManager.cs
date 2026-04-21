using UnityEngine;

public class EscapeProgressManager : MonoBehaviour
{
    public float currentXP=0;
    public float maxXP=500;
    public float progressCoefficient = 1f;
    public float coofReductionPerLevel=0.1f; // temp

    public int level = 0;
    public int maxLevel = 1;
    private float XPTillNextLevel;
    
    
    public Bar progressBar;
    
    public float onChangeTempXP=0;
    [HideInInspector] public float initialTempXp=float.MaxValue;

    public EnemiesSpawnManager enemiesSpawnManager;
    private DialogueManager  dialogueManager;
    private GameFlowManager gameFlowManager;

    public void Init()
    {
        currentXP = 0;
        progressCoefficient = 1f;
        level = 0;
        onChangeTempXP = 0;
        initialTempXp = float.MaxValue;
        
        Start();
    }
    private void Start()
    {
        gameFlowManager = FindFirstObjectByType<GameFlowManager>();
        dialogueManager =  FindFirstObjectByType<DialogueManager>();
        initialTempXp = float.MaxValue;
        if (!progressBar) progressBar = GameObject.Find("EscapeProgressBar").GetComponent<Bar>();
        enemiesSpawnManager = FindFirstObjectByType<EnemiesSpawnManager>();

        progressBar.CreateSpreadIndicators(maxLevel);
        XPTillNextLevel = maxXP / maxLevel;
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
        // CheckOverflow();
    }

    public void ChangeXP(float value, float newTimeToFadeFill = -1f)
    {
        enemiesSpawnManager.ProcessXP(value*progressCoefficient);

        float processedValue = value * progressCoefficient;
        currentXP += processedValue;
        XPTillNextLevel -= processedValue;
        
        if (newTimeToFadeFill >= 0f)
        {
            ChangeBarFillTime(newTimeToFadeFill);
        }
        UpdateBar();
        CheckOverflow();
    }

    public void CheckOverflow()
    {
        if (XPTillNextLevel > 0) return;

        UpdateLevel(true);

    }

    public void ChangeBarFillTime(float duration)
    {
        progressBar.drainDuration = duration;
        
    }

    public void UpdateLevel(bool nextLevel = true)
    {
        if  (nextLevel)
        {
            level++;
            h.Out(level);
            if (gameFlowManager.state == GameFlowManager.States.Finale) return;
            if (level >= maxLevel)
            {
                gameFlowManager.SetFinal();

                return;
            }
            
            h.Out(currentXP, "xp before");
            SetXP(maxXP / maxLevel * level, 0);
            h.Out(currentXP, "xp after");
            // Wait for progress bar to fill before talking
            h.InvokeAfterTime(this, progressBar.drainDuration, () =>
            {
                dialogueManager.GetComponent<Talkable>().Talk(level);
                SetXP(maxXP / maxLevel * level, 0);
            });
        }
        
        
        coofReductionPerLevel = Mathf.Clamp(Mathf.Pow(1-coofReductionPerLevel, level), 0, 1f);
        XPTillNextLevel = maxXP / maxLevel;
        XPTillNextLevel -= currentXP - (maxXP / maxLevel) * level;
    }

}
