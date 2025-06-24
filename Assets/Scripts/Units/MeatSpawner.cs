using UnityEngine;

public class MeatSpawner : MonoBehaviour
{
    [Header("Spawning Settings")]
    [SerializeField] private GameObject meatPrefab;
    [SerializeField] private int maxMeatOnMap = 5;
    [SerializeField] private float spawnInterval = 2f;

    [Header("Spawn Area")]
    [SerializeField] private Vector2 spawnAreaMin = new Vector2(-10f, -10f);
    [SerializeField] private Vector2 spawnAreaMax = new Vector2(10f, 10f);
    [SerializeField] private LayerMask obstacleLayer;

    private float nextSpawnTime;
    private int currentMeatCount;

    void Start()
    {
        nextSpawnTime = Time.time + spawnInterval;

        for (int i = 0; i < Mathf.Min(3, maxMeatOnMap); i++)
        {
            SpawnMeat();
        }
    }

    void Update()
    {
        currentMeatCount = GameObject.FindGameObjectsWithTag("Meat").Length;

        if (Time.time >= nextSpawnTime && currentMeatCount < maxMeatOnMap)
        {
            SpawnMeat();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnMeat()
    {
        Vector2 spawnPosition = GetRandomSpawnPosition();

        if (spawnPosition != Vector2.zero)
        {
            GameObject newMeat = Instantiate(meatPrefab, spawnPosition, Quaternion.identity);
            newMeat.tag = "Meat";

            Debug.Log($"Meat spawned at position: {spawnPosition}");
        }
    }

    Vector2 GetRandomSpawnPosition()
    {
        int attempts = 0;
        int maxAttempts = 20;

        while (attempts < maxAttempts)
        {
            Vector2 randomPosition = new Vector2(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                Random.Range(spawnAreaMin.y, spawnAreaMax.y)
            );

            Collider2D overlap = Physics2D.OverlapCircle(randomPosition, 0.5f, obstacleLayer);

            if (overlap == null)
            {
                return randomPosition;
            }

            attempts++;
        }

        Debug.LogWarning("Could not find valid spawn position for meat!");
        return Vector2.zero;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector2 center = (spawnAreaMin + spawnAreaMax) * 0.5f;
        Vector2 size = spawnAreaMax - spawnAreaMin;
        Gizmos.DrawWireCube(center, size);
    }
}