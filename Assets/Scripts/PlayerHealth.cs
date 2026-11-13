using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    public System.Action onHealthChanged;

    [Header("Возрождение")]
    public Transform respawnPoint;
    public float respawnTime = 3f;

    private bool isDead = false;
    private float respawnTimer = 0f;

    private Rigidbody rb;
    private PlayerMovement movementScript;
    private PlayerAbilities abilitiesScript;
    private MeshRenderer[] visuals;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody>();
        movementScript = GetComponent<PlayerMovement>();
        abilitiesScript = GetComponent<PlayerAbilities>();
        visuals = GetComponentsInChildren<MeshRenderer>();

        // ЖЕСТКИЙ ПОИСК ТОЧКИ СПАВНА
        if (respawnPoint == null)
        {
            GameObject blueBase = GameObject.FindGameObjectWithTag("BlueBase");
            if (blueBase != null)
            {
                respawnPoint = blueBase.transform;
                Debug.Log("Точка спавна не задана вручную, найдена база по тегу: " + respawnPoint.name);
            }
            else
            {
                Debug.LogError("КРИТИЧЕСКАЯ ОШИБКА: Не найдена синяя база для спавна!");
            }
        }
    }

    void Update()
    {
        if (isDead)
        {
            respawnTimer -= Time.unscaledDeltaTime; // Используем unscaledDeltaTime, чтобы таймер шел даже если игра на паузе
            if (respawnTimer <= 0)
            {
                Respawn();
            }
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        onHealthChanged?.Invoke();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (isDead) return;

        if (currentHealth < maxHealth)
        {
            currentHealth += amount;
            if (currentHealth > maxHealth) currentHealth = maxHealth;
            onHealthChanged?.Invoke();
        }
    }

    private void Die()
    {
        isDead = true;
        respawnTimer = respawnTime;

        if (movementScript != null) movementScript.enabled = false;
        if (abilitiesScript != null) abilitiesScript.enabled = false;

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        foreach (var mesh in visuals) mesh.enabled = false;

        Debug.Log("Игрок умер.");
    }

    private void Respawn()
    {
        isDead = false;

        // ПРИНУДИТЕЛЬНО ВОЗВРАЩАЕМ ВРЕМЯ (если кто-то его остановил)
        Time.timeScale = 1f;

        if (respawnPoint != null)
        {
            transform.position = respawnPoint.position;
            Debug.Log("Игрок телепортирован на: " + respawnPoint.name);
        }
        else
        {
            Debug.LogWarning("Некуда телепортировать, respawnPoint пуст!");
        }

        currentHealth = maxHealth;
        onHealthChanged?.Invoke();

        foreach (var mesh in visuals) mesh.enabled = true;

        if (rb != null) rb.isKinematic = false;
        if (movementScript != null) movementScript.enabled = true;
        if (abilitiesScript != null) abilitiesScript.enabled = true;

        Debug.Log("Игрок возродился!");
    }
}