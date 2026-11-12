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

    [Header("ЛКМ - Атака")]
    public float attackDamage = 20f;
    public float attackRange = 3f;
    public float attackCooldown = 0.5f;
    private float lastAttackTime = -10f;

    void Start()
    {
        movementScript = GetComponent<PlayerMovement>();
        healthScript = GetComponent<PlayerHealth>();

        // Запоминаем изначальную скорость
        originalSpeed = movementScript.moveSpeed;
    }

    void Update()
    {
        HandleAttack();
        HandleAbilities();
    }

    // --- АТАКА (ЛКМ) ---
    private void HandleAttack()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;

            // Ищем всех врагов в радиусе атаки
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
            bool hitSomething = false;

            foreach (var hitCollider in hitColliders)
            {
                // Ищем скрипт CreepAI на объекте
                CreepAI creep = hitCollider.GetComponent<CreepAI>();
                if (creep != null)
                {
                    // Поворачиваемся к крипу и бьем
                    Vector3 dir = (creep.transform.position - transform.position).normalized;
                    dir.y = 0; // Чтобы игрок не наклонялся
                    transform.rotation = Quaternion.LookRotation(dir);

                    creep.TakeDamage(attackDamage);
                    Debug.Log("Игрок ударил крипа!");
                    hitSomething = true;
                    break; // Бьем только одного за клик
                }
            }

            if (!hitSomething)
            {
                Debug.Log("Промах! Рядом нет врагов.");
            }
        }
    }

    // --- СПОСОБНОСТИ (E и R) ---
    private void HandleAbilities()
    {
        // Ускорение (E)
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

        // Лечение у базы (R)
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

    void TryHealAtBase()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, baseHealRadius);

        bool nearBase = false;
        foreach (Collider col in colliders)
        {
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