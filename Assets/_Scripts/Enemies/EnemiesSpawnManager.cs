using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemiesSpawnManager : MonoBehaviour
{
    [SerializeField] private bool useContainer = false;
    public GameObjectObjectsKindsContainer enemyPrefabsContainer;
    public List<GameObject> enemyPrefabs = new List<GameObject>();

    private EscapeProgressManager escapeProgressManager;

    private Transform scrollingParent;

    public int threatLevel = 1;

    public List<Transform> spawnPoints;
    public Transform deathPoint;

    [SerializeField] private float baseXpValue = 20f;
    [SerializeField] private float stepXP = 5f;
    public float XPPerEnemy = 20f; // ?

    public float XPQuota = 0;
    public float gainedXP = 0;

    public List<EnemyBase> enemies;

    public bool finalReached = false;
    private GameFlowManager gameFlowManager;

    private void Start()
    {
        if (!gameFlowManager) gameFlowManager =  FindFirstObjectByType<GameFlowManager>();
        if (useContainer)
        {
            enemyPrefabs = new List<GameObject>();
            enemyPrefabs.AddRange(enemyPrefabsContainer.objects.Values.ToList());
        }

        if (!scrollingParent) scrollingParent = GameObject.FindGameObjectWithTag("ScrollingParent").transform;
        // Find all objects with tag EnemiesSpawnPoint and add to spawnPoints list
        GameObject[] spawnPointObjects = GameObject.FindGameObjectsWithTag("EnemiesSpawnPoint");
        foreach (GameObject obj in spawnPointObjects)
        {
            spawnPoints.Add(obj.transform);
        }

        if (!deathPoint) deathPoint = GameObject.FindGameObjectWithTag("EnemiesDeathPoint").transform;

        // Find all enemies and add to list
        EnemyBase[] allEnemies = FindObjectsByType<EnemyBase>(FindObjectsSortMode.None);
        enemies.AddRange(allEnemies);

        escapeProgressManager = FindFirstObjectByType<EscapeProgressManager>();
        UpdateThreatLevel();
    }

    public void GenerateQuota()
    {
        XPQuota = h.RangeWithCoof(XPPerEnemy, 0.6f);
        if (XPQuota <= 0) XPQuota = baseXpValue;
    }

    public void ProcessXP(float XP)
    {
        gainedXP += XP;
        UpdateThreatLevel();
        if (gainedXP >= XPQuota)
        {
            gainedXP = h.Min(0, gainedXP - XPQuota);
            SpawnEnemy();
            GenerateQuota();
            // ProcessXP(0);
        }
    }

    public void UpdateThreatLevel()
    {
        threatLevel = (int)escapeProgressManager.currentXP / 100 + 1;
        XPPerEnemy = baseXpValue - stepXP * (threatLevel - 1);
        if (XPQuota <= 0) XPQuota = baseXpValue;
        // update enemies mb
        if (escapeProgressManager.currentXP >= escapeProgressManager.maxXP)
        {
            finalReached = true;
            Finale();
        }
    }

    public void Finale()
    {
        h.Out("FINALe");
        //TODO
    }

    public GameObject SpawnEnemy(GameObject enemyPrefab = null, bool ignoreConditions=false)
    {
        if (!ignoreConditions && (finalReached ||  gameFlowManager.state == GameFlowManager.States.Finale)) return default;

        if (!enemyPrefab) enemyPrefab = h.RandChoice(enemyPrefabs);
        GameObject enemy = null;
        Transform spawnPoint = h.RandChoice(spawnPoints);
        if (enemyPrefab && spawnPoint)
        {
            enemy = Instantiate(
                enemyPrefab,
                spawnPoint.position,
                enemyPrefab.transform.rotation,
                scrollingParent);

            //TEMP
            if (enemy.GetComponent<EnemyBase>() is PoliceEnemy policeEnemy)
                policeEnemy.transform.localPosition =
                    new Vector3(enemy.transform.localPosition.x, 0, enemy.transform.localPosition.z);
        }


        if (enemy && enemy.TryGetComponent(out EnemyBase enemyComp)) enemies.Add(enemyComp);

        return enemy;
    }

    public void CheckDeath()
    {

        List<EnemyBase> toRemove = new List<EnemyBase>();
        foreach (EnemyBase enemy in enemies)
        {
            if (!enemy)
            {
                toRemove.Add(enemy);
                continue;
            }

            if (enemy.transform.position.x <= deathPoint.position.x)
            {
                enemy.Death();
                toRemove.Add(enemy);
            }

        }

        foreach (var enemy in toRemove)
        {
            enemies.Remove(enemy);
        }
    }

    public void ClearEnemies()
    {
        foreach (EnemyBase enemy in enemies)
        {
            if (enemy)
            {
                Destroy(enemy.gameObject);
            }
        }

        enemies.Clear();
    }
}