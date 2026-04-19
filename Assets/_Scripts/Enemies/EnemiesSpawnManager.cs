using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemiesSpawnManager : MonoBehaviour
{
    public GameObjectObjectsKindsContainer enemyPrefabs;
    private EscapeProgressManager escapeProgressManager;
    

    public int threatLevel = 1;

    public List<Transform> spawnPoints;

    [SerializeField] private float baseXpValue = 20f;
    [SerializeField] private float stepXP = 5f;
    public float XPPerEnemy=20f;// ?

    public float XPQuota=0;
    public float gainedXP = 0;
    private void Start()
    {
        // Find all objects with tag EnemiesSpawnPoint and add to spawnPoints list
        GameObject[] spawnPointObjects = GameObject.FindGameObjectsWithTag("EnemiesSpawnPoint");
        foreach (GameObject obj in spawnPointObjects)
        {
            spawnPoints.Add(obj.transform);
        }
        
        escapeProgressManager = FindFirstObjectByType<EscapeProgressManager>();
        UpdateThreatLevel();
    }

    public void GenerateQuota()
    {
        XPQuota = h.RangeWithCoof(XPPerEnemy, 0.6f);
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
            ProcessXP(0);
        }
    }
    
    public void UpdateThreatLevel()
    {
        threatLevel = (int)escapeProgressManager.currentXP / 100 + 1;
        XPPerEnemy = baseXpValue + stepXP * (threatLevel - 1);
        // update enemies mb
    }

    public GameObject SpawnEnemy(GameObject enemyPrefab = null)
    {
        if (!enemyPrefab) enemyPrefab = h.RandChoice(enemyPrefabs.objects.Values.ToList());
        GameObject enemy = null;
        Transform spawnPoint = h.RandChoice(spawnPoints);
        if (enemyPrefab && spawnPoint)
            enemy = Instantiate(
                enemyPrefab, 
                spawnPoint.position,
                enemyPrefab.transform.rotation,
                GameObject.FindGameObjectWithTag("ScrollingParent").transform);
        
        return enemy;
    }
    
}
