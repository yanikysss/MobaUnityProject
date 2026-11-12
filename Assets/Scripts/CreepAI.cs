using UnityEngine;

public class CreepAI : MonoBehaviour
{
    [Header("Настройки")]
    public float moveSpeed = 3f;
    public float attackRange = 2f;
    public float attackDamage = 10f;
    public float attackCooldown = 1.5f;
    public float health = 50f;

    private Transform currentTarget;
    private float lastAttackTime = -10f;
    private float lastTargetSearchTime = 0f;

    void Update()
    {
        // Каждые 0.5 секунды ищем цель заново (вдруг игрок убежал или подошел ближе)
        if (Time.time >= lastTargetSearchTime + 0.5f)
        {
            FindTarget();
            lastTargetSearchTime = Time.time;
        }

        if (currentTarget != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);

            // Если цель далеко — идем к ней
            if (distanceToTarget > attackRange)
            {
                Vector3 direction = (currentTarget.position - transform.position).normalized;
                direction.y = 0; // Чтобы не лететь по воздуху
                transform.position += direction * moveSpeed * Time.deltaTime;

                // Поворачиваем крипа в сторону цели
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

    void FindTarget()
    {
        // 1. В ПРИОРИТЕТЕ ищем игрока (по тегу Player)
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null && player.activeInHierarchy)
        {
            currentTarget = player.transform;
            return;
        }

        // 2. Если игрока нет (или он умер), ищем синюю базу (по тегу BlueBase)
        GameObject blueBase = GameObject.FindGameObjectWithTag("BlueBase");
        if (blueBase != null && blueBase.activeInHierarchy)
        {
            currentTarget = blueBase.transform;
        }
    }

    void AttackTarget()
    {
        // Наносим урон игроку
        if (currentTarget.CompareTag("Player"))
        {
            PlayerHealth ph = currentTarget.GetComponent<PlayerHealth>();
            if (ph != null) ph.TakeDamage(attackDamage);
        }
        // Наносим урон базе
        else if (currentTarget.CompareTag("BlueBase"))
        {
            BaseHealth bh = currentTarget.GetComponent<BaseHealth>();
            if (bh != null) bh.TakeDamage(attackDamage);
        }
    }

    // Крип получает урон от игрока (вызовется из скрипта игрока)
    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Destroy(gameObject); // Крип умирает
        }
    }
}