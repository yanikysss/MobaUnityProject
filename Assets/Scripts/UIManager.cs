using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance; // Синглтон для доступа из GameManager

    [Header("UI Texts (TextMeshPro)")]
    public TextMeshProUGUI playerHealthText;
    public TextMeshProUGUI killsText;
    public TextMeshProUGUI blueBaseText;
    public TextMeshProUGUI redBaseText;

    [Header("End Game UI")]
    public GameObject endGamePanel;          // Панель с результатом (скрыта в начале)
    public TextMeshProUGUI endGameResultText; // Текст "ПОБЕДА" или "ПОРАЖЕНИЕ"
    public TextMeshProUGUI endGameDeathsText; // Текст "Смертей: X"
    public GameObject restartButton;          // Кнопка перезапуска (скрыта в начале)

    [Header("Scene References")]
    public PlayerHealth playerHealth;
    public BaseHealth blueBase;
    public BaseHealth redBase;

    private int killsCount = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Подписки на события
        if (playerHealth != null) playerHealth.onHealthChanged += UpdateUI;
        if (blueBase != null) blueBase.onHealthChanged += UpdateUI;
        if (redBase != null) redBase.onHealthChanged += UpdateUI;

        CreepAI.onCreepKilled += OnCreepKilled;

        UpdateUI();
        UpdateKillsUI();

        // Скрываем экран окончания игры и кнопку при старте
        if (endGamePanel != null) endGamePanel.SetActive(false);
        if (restartButton != null) restartButton.SetActive(false);

        InvokeRepeating(nameof(UpdateUI), 0.5f, 0.5f);
    }

    void OnDestroy()
    {
        CreepAI.onCreepKilled -= OnCreepKilled;
    }

    void UpdateUI()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameOver) return;

        if (playerHealthText != null && playerHealth != null && playerHealth.gameObject.activeInHierarchy)
            playerHealthText.text = "HP: " + Mathf.RoundToInt(playerHealth.currentHealth);

        if (blueBaseText != null && blueBase != null && blueBase.gameObject.activeInHierarchy)
            blueBaseText.text = "Blue Base: " + Mathf.RoundToInt(blueBase.currentHealth);

        if (redBaseText != null && redBase != null && redBase.gameObject.activeInHierarchy)
            redBaseText.text = "Red Base: " + Mathf.RoundToInt(redBase.currentHealth);
    }

    void OnCreepKilled()
    {
        killsCount++;
        UpdateKillsUI();
    }

    void UpdateKillsUI()
    {
        if (killsText != null)
            killsText.text = "Kills: " + killsCount;
    }

    // Вызывается из GameManager при победе или поражении
    public void ShowGameOverScreen(bool isVictory)
    {
        if (endGamePanel != null)
        {
            endGamePanel.SetActive(true);

            if (endGameResultText != null)
            {
                if (isVictory)
                {
                    endGameResultText.text = "ПОБЕДА!";
                    endGameResultText.color = Color.green;
                }
                else
                {
                    endGameResultText.text = "ПОРАЖЕНИЕ!";
                    endGameResultText.color = Color.red;
                }
            }

            if (endGameDeathsText != null)
            {
                endGameDeathsText.text = "Смертей: " + PlayerHealth.deathsCount;
                endGameDeathsText.color = Color.white;
            }

            // Показываем кнопку перезапуска
            if (restartButton != null)
            {
                restartButton.SetActive(true);
            }
        }
    }
}