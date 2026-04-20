using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameFlowManager : MonoBehaviour
{
    public List<OrganBase> organsOnStart = new List<OrganBase>();

    private UpgradeManager upgradeManager;
    private EnemiesSpawnManager enemiesSpawnManager;

    [SerializeField] private float restartDuration = 2f;
    
    public enum States
    {
        Intro,
        Game,
        Pause,
        Loss
    }

    public States state = States.Intro;

    private void Awake()
    {
        upgradeManager = GameObject.FindFirstObjectByType<UpgradeManager>();
        enemiesSpawnManager = GameObject.FindFirstObjectByType<EnemiesSpawnManager>();
    }

    public void StartGame(bool firstTime = true)
    {
        Restart();
    }
    
    public void SetOnPause()
    {
        state = States.Pause;
        h.Out("oause");
    }

    public void SetOnGame()
    {
        state = States.Game;
        h.Out("game");
    }

    public void SetOnLoss()
    {
        state = States.Pause;
        h.Out("loss");
        
        //change sprite
        GameObject playerParent = GameObject.FindGameObjectWithTag("PlayerParent");
        playerParent.SetActive(false);
        
        GameObject damageScreen = GameObject.Find("DamageScreen");
        Color lossColor = damageScreen.GetComponent<Image>().color;
        lossColor.a = 1f;
        damageScreen.GetComponent<Image>().color = lossColor;
        
        GameObject restartWindow = GameObject.Find("RestartWindow");
        if (restartWindow)
        {
            restartWindow.SetActive(true);
        }
    }

    public void Restart()
    {
        StartCoroutine(RestartCoroutine());
    }

    public IEnumerator RestartCoroutine()
    {
        yield return StartCoroutine(ScreenManager.Instance.FadeRoutine(0, 1, restartDuration/2));
        
        h.Out("restart");
        
        GameObject restartWindow = GameObject.Find("RestartWindow");
        if (restartWindow)
        {
            restartWindow.SetActive(false);
        }
        
        GameObject playerParent = GameObject.FindGameObjectWithTag("PlayerParent");
        
        playerParent.SetActive(true);
        
        
        enemiesSpawnManager.ClearEnemies();
        upgradeManager.ClearNeuronsAndOrgans();

        foreach (OrganBase organ in organsOnStart)
        {
            upgradeManager.AddOrgan(organ);
        }
        
        // update cellmanager
        // update xpmanager
        FindFirstObjectByType<EscapeProgressManager>().Init();
        // update bpm manager
        //update damage manager
        
        
        
        yield return StartCoroutine(ScreenManager.Instance.FadeRoutine(1, 0, restartDuration/2));
        SetOnGame();
    }

    public bool IsPaused()
    {
        return state == States.Pause;
    }
}
