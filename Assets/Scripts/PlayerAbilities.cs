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
        originalSpeed = movementScript.moveSpeed;
    }

    void Update()
    {
        HandleAttack();
        HandleAbilities();
    }

    private void HandleAttack()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
            bool hitSomething = false;

            foreach (var hitCollider in hitColliders)
            {
                CreepAI creep = hitCollider.GetComponent<CreepAI>();
                if (creep != null)
                {
                    Vector3 dir = (creep.transform.position - transform.position).normalized;
                    dir.y = 0;
                    transform.rotation = Quaternion.LookRotation(dir);

                    creep.TakeDamage(attackDamage);
                    Debug.Log("Игрок ударил крипа!");
                    hitSomething = true;
                    break;
                }

                if (hitCollider.CompareTag("RedBase"))
                {
                    BaseHealth baseHealth = hitCollider.GetComponent<BaseHealth>();
                    if (baseHealth != null)
                    {
                        Vector3 dir = (hitCollider.transform.position - transform.position).normalized;
                        dir.y = 0;
                        transform.rotation = Quaternion.LookRotation(dir);

                        baseHealth.TakeDamage(attackDamage);
                        Debug.Log("Игрок атакует КРАСНУЮ базу!");
                        hitSomething = true;
                        break;
                    }
                }
            }

            if (!hitSomething)
            {
                Debug.Log("Промах! Рядом нет врагов или красной базы.");
            }
        }
    }

    private void HandleAbilities()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Time.time >= lastSpeedBoostTime + speedCooldown && !isSpeedBoosted)
            {
                ActivateSpeedBoost();
            }
            else if (isSpeedBoosted) Debug.Log("Ускорение уже активно!");
            else Debug.Log("Ускорение на кулдауне!");
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (Time.time >= lastHealTime + healCooldown)
            {
                TryHealAtBase();
            }
            else Debug.Log("Лечение на кулдауне!");
        }
    }

    void ActivateSpeedBoost()
    {
        lastSpeedBoostTime = Time.time;
        isSpeedBoosted = true;
        movementScript.moveSpeed = originalSpeed * speedMultiplier;
        Invoke(nameof(ResetSpeed), speedDuration);
    }

    void ResetSpeed()
    {
        movementScript.moveSpeed = originalSpeed;
        isSpeedBoosted = false;
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
            lastHealTime = Time.time;
            Debug.Log("Вылечен на базе!");
        }
        else if (!nearBase)
        {
            Debug.Log("Слишком далеко от синей базы!");
        }
    }
}