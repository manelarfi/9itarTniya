using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [Header("Level Generation Settings")]
    public GameObject[] levelPrefabs; // Array of level prefabs to spawn
    public Transform spawnPoint; // The stationary spawn position
    public float levelLength = 50f; // The Z-axis spacing between levels
    public int maxActiveLevels = 10; // Maximum number of active levels

    private Queue<GameObject> activeLevels = new Queue<GameObject>();

    void Start()
    {
        // Subscribe to the event
        EventManager.Instance.OnGenerateNewLevel += SpawnLevel;

        // Spawn the initial level
        SpawnLevel();
    }

    void OnDestroy()
    {
        // Unsubscribe from the event to avoid memory leaks
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnGenerateNewLevel -= SpawnLevel;
        }
    }

    /// <summary>
    /// Spawns a new level prefab.
    /// </summary>
    private void SpawnLevel()
    {
        if (levelPrefabs == null || levelPrefabs.Length == 0)
        {
            Debug.LogWarning("No level prefabs assigned to the PlatformSpawner.");
            return;
        }

        // Choose a random level prefab
        GameObject levelToSpawn = levelPrefabs[Random.Range(0, levelPrefabs.Length)];

        // Instantiate the level prefab at the spawn position
        GameObject spawnedLevel = Instantiate(levelToSpawn, spawnPoint.position, Quaternion.identity);

        // Add the new level to the active levels queue
        activeLevels.Enqueue(spawnedLevel);

        // Remove old levels if exceeding max active levels
        if (activeLevels.Count > maxActiveLevels)
        {
            Destroy(activeLevels.Dequeue());
        }
    }
}
