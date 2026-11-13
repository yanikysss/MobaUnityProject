using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI Texts (TextMeshPro)")]
    public TextMeshProUGUI playerHealthText;
    public TextMeshProUGUI killsText;
    public TextMeshProUGUI blueBaseText;
    public TextMeshProUGUI redBaseText;

    [Header("Scene References")]
    public PlayerHealth playerHealth;
    public BaseHealth blueBase;
    public BaseHealth redBase;

    private int killsCount = 0;

    void Start()
    {
        if (playerHealth != null) playerHealth.onHealthChanged += UpdateUI;
        if (blueBase != null) blueBase.onHealthChanged += UpdateUI;
        if (redBase != null) redBase.onHealthChanged += UpdateUI;

        CreepAI.onCreepKilled += OnCreepKilled;

        // Принудительное обновление при старте
        UpdateUI();
        UpdateKillsUI();

        // На всякий случай обновляем UI каждые 0.5 сек (если событие вдруг не сработает)
        InvokeRepeating(nameof(UpdateUI), 0.5f, 0.5f);
    }

    void OnDestroy()
    {
        CreepAI.onCreepKilled -= OnCreepKilled;
    }

    void UpdateUI()
    {
        if (playerHealthText != null && playerHealth != null)
            playerHealthText.text = "HP: " + Mathf.RoundToInt(playerHealth.currentHealth);

        if (blueBaseText != null && blueBase != null)
            blueBaseText.text = "Blue Base: " + Mathf.RoundToInt(blueBase.currentHealth);

        if (redBaseText != null && redBase != null)
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
}