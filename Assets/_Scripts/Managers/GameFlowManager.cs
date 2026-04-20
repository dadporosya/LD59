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
    
    [SerializeField] private GameObject restartWindow;
    [SerializeField] private GameObject playerParent;

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
        restartWindow =  GameObject.Find("RestartWindow");
        playerParent =  GameObject.FindGameObjectWithTag("PlayerParent");

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            SetOnLoss();
        }
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
        playerParent.SetActive(false);
        
        GameObject damageScreen = GameObject.Find("DamageScreen");
        Color lossColor = damageScreen.GetComponent<Image>().color;
        lossColor.a = 1f;
        damageScreen.GetComponent<Image>().color = lossColor;
        
        h.Out(restartWindow);
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
        
        if (restartWindow)
        {
            restartWindow.SetActive(false);
        }
        
        playerParent.SetActive(true);
        
        enemiesSpawnManager.ClearEnemies();
        upgradeManager.ClearNeuronsAndOrgans();

        foreach (OrganBase organ in organsOnStart)
        {
            upgradeManager.AddOrgan(organ);
        }
        
        // update cellmanager
        FindFirstObjectByType<CellsManager>().Init();
        
        // update xpmanager
        FindFirstObjectByType<EscapeProgressManager>().Init();
        
        // update bpm manager
        FindFirstObjectByType<BPMManager>().Init();
        
        //update damage manager
        FindFirstObjectByType<PlayerDamageManager>().Init();
        
        
        yield return StartCoroutine(ScreenManager.Instance.FadeRoutine(1, 0, restartDuration/2));
        SetOnGame();
    }

    public bool IsPaused()
    {
        return state != States.Game;
    }
}
