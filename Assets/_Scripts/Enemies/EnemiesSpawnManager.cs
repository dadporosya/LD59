using System.Linq;
using UnityEngine;

public class EnemiesSpawnManager : MonoBehaviour
{
    public GameObjectObjectsKindsContainer enemyPrefabs;
    private EscapeProgressManager escapeProgressManager;

    public int threatLevel = 1;

    public Transform spawnPoint;
    private void Start()
    {
        escapeProgressManager = FindFirstObjectByType<EscapeProgressManager>();
    }

    public void UpdateThreatLevel()
    {
        threatLevel = (int)escapeProgressManager.currentXP / 100 + 1;
        
        // update enemies mb
    }

    public GameObject SpawnEnemy(GameObject enemyPrefab = null)
    {
        if (!enemyPrefab) enemyPrefab = h.RandChoice(enemyPrefabs.objects.Values.ToList());
        
    }
    
}
