using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager Instance;

    private void Awake()
    {
        // Делаем этот объект доступным из любого скрипта
        Instance = this;
    }

    public void StartRespawn(GameObject player, float delay, Transform spawnPoint, System.Action onRespawnComplete)
    {
        // Запускаем корутину (таймер), которая не зависит от объекта игрока
        StartCoroutine(RespawnCoroutine(player, delay, spawnPoint, onRespawnComplete));
    }

    private System.Collections.IEnumerator RespawnCoroutine(GameObject player, float delay, Transform spawnPoint, System.Action onRespawnComplete)
    {
        yield return new WaitForSeconds(delay); // Ждем указанное время

        // Телепортируем игрока
        if (spawnPoint != null)
        {
            player.transform.position = spawnPoint.position;
        }

        // Вызываем коллбэк, чтобы вернуть здоровье и включить управление
        onRespawnComplete?.Invoke();
    }
}