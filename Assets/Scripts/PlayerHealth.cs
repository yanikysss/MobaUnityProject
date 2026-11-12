using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    void Start()
    {
        currentHealth = maxHealth; // При старте здоровье полное
    }

    // Метод для получения урона (пригодится, когда добавим врагов)
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log("Игрок получил урон. Текущее HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Метод для лечения (его вызывает способность T)
    public void Heal(float amount)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += amount;
            // Ограничиваем здоровье максимумом
            if (currentHealth > maxHealth) currentHealth = maxHealth;
            Debug.Log("Игрок вылечился. Текущее HP: " + currentHealth);
        }
        else
        {
            Debug.Log("Здоровье уже полное!");
        }
    }

    private void Die()
    {
        Debug.Log("Игрок погиб!");
        // Пока просто отключаем объект
        gameObject.SetActive(false);
    }
}