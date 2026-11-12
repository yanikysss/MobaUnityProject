using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerAbilities : MonoBehaviour
{
    private PlayerMovement movementScript;
    private PlayerHealth healthScript;

    [Header("E - Ускорение")]
    public float speedMultiplier = 1.8f;
    public float speedDuration = 3f;
    public float speedCooldown = 5f;
    private float originalSpeed;
    private bool isSpeedBoosted = false;
    private float lastSpeedBoostTime = -10f;

    [Header("R - Лечение у базы")]
    public float healAmount = 30f;
    public float baseHealRadius = 8f;
    public float healCooldown = 8f;
    private float lastHealTime = -10f;

    void Start()
    {
        movementScript = GetComponent<PlayerMovement>();
        healthScript = GetComponent<PlayerHealth>();

        // Запоминаем изначальную скорость
        originalSpeed = movementScript.moveSpeed;
    }

    void Update()
    {
        // Способность 1: Ускорение (E)
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Time.time >= lastSpeedBoostTime + speedCooldown && !isSpeedBoosted)
            {
                ActivateSpeedBoost();
            }
            else if (isSpeedBoosted)
            {
                Debug.Log("Ускорение уже активно!");
            }
            else
            {
                Debug.Log("Ускорение на кулдауне! Осталось: " + (speedCooldown - (Time.time - lastSpeedBoostTime)).ToString("F1") + " сек.");
            }
        }

        // Способность 2: Лечение у базы (R)
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (Time.time >= lastHealTime + healCooldown)
            {
                TryHealAtBase();
            }
            else
            {
                Debug.Log("Лечение на кулдауне! Осталось: " + (healCooldown - (Time.time - lastHealTime)).ToString("F1") + " сек.");
            }
        }
    }

    // --- ЛОГИКА УСКОРЕНИЯ ---
    void ActivateSpeedBoost()
    {
        lastSpeedBoostTime = Time.time; // Запускаем КД
        isSpeedBoosted = true;
        movementScript.moveSpeed = originalSpeed * speedMultiplier;
        Invoke(nameof(ResetSpeed), speedDuration);
        Debug.Log("Способность E: Ускорение активировано!");
    }

    void ResetSpeed()
    {
        movementScript.moveSpeed = originalSpeed;
        isSpeedBoosted = false;
        Debug.Log("Способность E: Ускорение закончилось.");
    }

    // --- ЛОГИКА ЛЕЧЕНИЯ У БАЗЫ (Клавиша R) ---
    void TryHealAtBase()
    {
        // Ищем все коллайдеры рядом с игроком в радиусе baseHealRadius
        Collider[] colliders = Physics.OverlapSphere(transform.position, baseHealRadius);

        bool nearBase = false;
        foreach (Collider col in colliders)
        {
            // Проверяем, есть ли рядом объект с тегом BlueBase
            if (col.CompareTag("BlueBase"))
            {
                nearBase = true;
                break;
            }
        }

        if (nearBase && healthScript != null)
        {
            healthScript.Heal(healAmount);
            lastHealTime = Time.time; // Запускаем КД только если лечение прошло успешно
            Debug.Log("Способность R: Вылечен на базе на " + healAmount + " HP!");
        }
        else if (!nearBase)
        {
            Debug.Log("Способность R: Слишком далеко от синей базы!");
        }
    }
}