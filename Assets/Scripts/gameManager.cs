using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Условия победы и поражения")]
    public int killsToWin = 5;
    public bool isGameOver = false;

    [Header("Ссылки на объекты")]
    public BaseHealth blueBase;
    public BaseHealth redBase;

    private int currentKills = 0;

    private void Awake()
    {
        // Делаем GameManager доступным из любого скрипта
        Instance = this;
    }

    private void OnEnable()
    {
        // Подписываемся на события
        CreepAI.onCreepKilled += OnCreepKilled;
    }

    private void OnDisable()
    {
        CreepAI.onCreepKilled -= OnCreepKilled;
    }

    private void Update()
    {
        if (isGameOver) return;

        // Проверка на поражение (если синяя база уничтожена)
        if (blueBase != null && blueBase.currentHealth <= 0)
        {
            EndGame(false); // Поражение
        }
        // Проверка на победу (если красная база уничтожена)
        else if (redBase != null && redBase.currentHealth <= 0)
        {
            EndGame(true); // Победа
        }
    }

    private void OnCreepKilled()
    {
        if (isGameOver) return;

        currentKills++;
        Debug.Log("Крипов убито: " + currentKills);

        // Проверка на победу по убийствам
        if (currentKills >= killsToWin)
        {
            EndGame(true); // Победа
        }
    }

    private void EndGame(bool isVictory)
    {
        isGameOver = true;

        // Останавливаем время (мир замирает)
        Time.timeScale = 0f;

        // Вызываем UI для показа экрана
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowGameOverScreen(isVictory);
        }
    }
}