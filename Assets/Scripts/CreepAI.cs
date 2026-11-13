using UnityEngine;

public class CreepAI : MonoBehaviour
{
    [Header("Движение и Атака")]
    public float moveSpeed = 3f;
    public float attackRange = 2f;
    public float attackDamage = 10f;
    public float attackCooldown = 1.5f;
    public float health = 50f;

    [Header("Агр")]
    public float aggroRange = 8f; // На каком расстоянии крип замечает игрока
    public float deAggroRange = 12f; // На каком расстоянии крип забывает игрока

    public static System.Action onCreepKilled;

    private Transform currentTarget;
    private Transform defaultTarget; // Синяя база (цель по умолчанию)
    private float lastAttackTime = -10f;
    private float lastTargetSearchTime = 0f;

    void Start()
    {
        // Находим синюю базу один раз при старте
        GameObject blueBase = GameObject.FindGameObjectWithTag("BlueBase");
        if (blueBase != null) defaultTarget = blueBase.transform;
    }

    void Update()
    {
        if (Time.time >= lastTargetSearchTime + 0.2f)
        {
            EvaluateTarget();
            lastTargetSearchTime = Time.time;
        }

        if (currentTarget != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);

            // Если цель далеко — идем к ней
            if (distanceToTarget > attackRange)
            {
                Vector3 direction = (currentTarget.position - transform.position).normalized;
                direction.y = 0;
                transform.position += direction * moveSpeed * Time.deltaTime;

                // Поворот
                if (direction != Vector3.zero)
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 10f * Time.deltaTime);
            }
            // Если цель близко — атакуем
            else
            {
                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    AttackTarget();
                    lastAttackTime = Time.time;
                }
            }
        }
    }

    void EvaluateTarget()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        // 1. Проверяем игрока
        if (playerObj != null && playerObj.activeInHierarchy)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerObj.transform.position);

            // Если игрок близко (в радиусе агра) — агримся на него
            if (distanceToPlayer <= aggroRange)
            {
                currentTarget = playerObj.transform;
                return;
            }
            // Если мы уже бежим за игроком, но он убежал слишком далеко — сбрасываем агр
            else if (currentTarget == playerObj.transform && distanceToPlayer > deAggroRange)
            {
                currentTarget = defaultTarget; // Переключаемся обратно на базу
                return;
            }
        }

        // 2. Если игрок не найден или далеко, а текущей цели нет — идем на базу
        if (currentTarget == null || (currentTarget != playerObj.transform && currentTarget != defaultTarget))
        {
            currentTarget = defaultTarget;
        }
    }

    void AttackTarget()
    {
        if (currentTarget.CompareTag("Player"))
        {
            PlayerHealth ph = currentTarget.GetComponent<PlayerHealth>();
            if (ph != null) ph.TakeDamage(attackDamage);
        }
        else if (currentTarget.CompareTag("BlueBase"))
        {
            BaseHealth bh = currentTarget.GetComponent<BaseHealth>();
            if (bh != null) bh.TakeDamage(attackDamage);
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            onCreepKilled?.Invoke();
            Destroy(gameObject);
        }
    }
}