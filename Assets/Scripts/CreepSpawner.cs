using UnityEngine;

public class CreepSpawner : MonoBehaviour
{
    public GameObject creepPrefab;
    public float spawnInterval = 5f; // Каждые 5 секунд
    public Transform spawnPoint; // Точка у красной базы

    void Start()
    {
        // Запускаем спавн каждые spawnInterval секунд
        InvokeRepeating(nameof(SpawnCreep), 2f, spawnInterval);
    }

    void SpawnCreep()
    {
        if (creepPrefab != null && spawnPoint != null)
        {
            Instantiate(creepPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
}