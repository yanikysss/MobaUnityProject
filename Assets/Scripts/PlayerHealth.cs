using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    // Событие для UI
    public System.Action onHealthChanged;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log("Игрок получил урон. Текущее HP: " + currentHealth);

        onHealthChanged?.Invoke(); // Вызываем событие

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += amount;
            if (currentHealth > maxHealth) currentHealth = maxHealth;
            Debug.Log("Игрок вылечился. Текущее HP: " + currentHealth);

            onHealthChanged?.Invoke(); // Вызываем событие
        }
        else
        {
            Debug.Log("Здоровье уже полное!");
        }
    }

    private void Die()
    {
        Debug.Log("Игрок погиб!");
        gameObject.SetActive(false);
    }
}   