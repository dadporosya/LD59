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
    
    [SerializeField] public GameObject restartWindow;
    [SerializeField] public GameObject playerParent;
    

    [SerializeField] public float restartDuration = 2f;
    
    public Vector3 cameraInitialPosition;
    private CutSceneManager cutSceneManager;

    public string currentDeathMessage;
    public string heartStopDeathMessage;
    public string tooFarPushHeartDeathMessage;

    public GameObject lossScreenPrefab;
    private GameObject currentLossScreen;

    private Coroutine damageScreenFadeCoroutine;
    
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
        cutSceneManager = FindFirstObjectByType<CutSceneManager>();
        upgradeManager = GameObject.FindFirstObjectByType<UpgradeManager>();
        enemiesSpawnManager = GameObject.FindFirstObjectByType<EnemiesSpawnManager>();
        restartWindow =  GameObject.Find("RestartWindow");
        playerParent =  GameObject.FindGameObjectWithTag("PlayerParent");
        
        cameraInitialPosition = Camera.main.transform.position;
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
        h.Out("CURENT LOSSS!!!!!!!_--------------------------------------", state);
        // cutSceneManager.RunCutscene("LossCutscene");
        if (state == States.Loss) return;
        state = States.Loss;
        h.Out("ANOTHER LOSSS!!!!!!!_--------------------------------------", state);
        
        h.Out(currentDeathMessage);
        ProcessLoss();
    }

    public void ProcessLoss()
    {
        state = States.Loss;
        h.Out("loss");
        
        //change sprite
        playerParent.SetActive(false);
        
        GameObject damageScreen = GameObject.Find("DamageScreen");
        FadeDamageScreen(damageScreen.GetComponent<Image>().color.a, 0.99f, 5f);
        
        h.Out(restartWindow);
        if (restartWindow)
        {
            restartWindow.SetActive(true);
        }
        
        h.Out("LOSS");
        if (currentLossScreen) Destroy(currentLossScreen);
        currentLossScreen = Instantiate(
            lossScreenPrefab, playerParent.transform.position,
            Quaternion.identity
        );
    }

   

    private IEnumerator FadeDamageScreenCoroutine(Image damageImage, float fromAlpha, float toAlpha, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(fromAlpha, toAlpha, elapsed / duration);
            Color color = damageImage.color;
            color.a = alpha;
            damageImage.color = color;
            yield return null;
        }

        Color finalColor = damageImage.color;
        finalColor.a = toAlpha;
        damageImage.color = finalColor;
    }

    public void Restart()
    {
        StartCoroutine(RestartCoroutine());
    }

    public IEnumerator RestartCoroutine()
    {
        yield return StartCoroutine(ScreenManager.Instance.FadeRoutine(0, 1, restartDuration/2));
        playerParent.SetActive(true);
        
        // update cellmanager
        FindFirstObjectByType<CellsManager>().Init();
        
        // update xpmanager
        FindFirstObjectByType<EscapeProgressManager>().Init();
        
        // update bpm manager
        FindFirstObjectByType<BPMManager>().Init();
        
        //update damage manager
        FindFirstObjectByType<PlayerDamageManager>().Init();

        upgradeManager.Init();
        
        
        h.Out("loss screen", currentLossScreen);
        if (currentLossScreen) Destroy(currentLossScreen);
        
        if (damageScreenFadeCoroutine != null) StopCoroutine(damageScreenFadeCoroutine);
        
        GameObject damageScreen = GameObject.Find("DamageScreen");
        Color lossColor = damageScreen.GetComponent<Image>().color;
        lossColor.a = 0f;
        damageScreen.GetComponent<Image>().color = lossColor;
        
        h.Out("restart", damageScreen, lossColor);
        
        if (restartWindow)
        {
            restartWindow.SetActive(false);
        }
        
        
        
        // Reset camera to initial position
        Camera.main.transform.position = cameraInitialPosition;
        
        ClearAllConnections();
        enemiesSpawnManager.ClearEnemies();
        upgradeManager.ClearNeuronsAndOrgans();

        foreach (OrganBase organ in organsOnStart)
        {
            upgradeManager.AddOrgan(organ);
        }
        
        
        
        h.Out("restart", damageScreen, damageScreen.GetComponent<Image>().color);
        damageScreen.GetComponent<Image>().color = lossColor;
        h.Out("restart", damageScreen, damageScreen.GetComponent<Image>().color);
        
        
        
        yield return StartCoroutine(ScreenManager.Instance.FadeRoutine(1, 0, restartDuration/2));
        SetOnGame();
    }

    public bool IsPaused()
    {
        return state != States.Game;
    }
    
    private void FadeDamageScreen(float fromAlpha, float toAlpha, float duration)
    {
        GameObject damageScreen = GameObject.Find("DamageScreen");
        if (!damageScreen) return;
        
        Image damageImage = damageScreen.GetComponent<Image>();
        if (!damageImage) return;
        
        damageScreenFadeCoroutine = StartCoroutine(FadeDamageScreenCoroutine(damageImage, fromAlpha, toAlpha, duration));
    }

    public void ClearAllConnections()
    {
        Connection[] allConnections = FindObjectsOfType<Connection>();
        foreach (Connection connection in allConnections)
        {
            connection.OnDestroy();
        }
    }
}
