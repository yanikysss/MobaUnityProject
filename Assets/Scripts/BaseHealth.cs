using UnityEngine;

public class BaseHealth : MonoBehaviour
{
    public float maxHealth = 500f;
    public float currentHealth; // Сделали public, чтобы UI мог читать

    // Событие для UI
    public System.Action onHealthChanged;

    private bool isDestroyed = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (isDestroyed) return;

        currentHealth -= amount;
        Debug.Log(gameObject.name + " получила урон! Осталось HP: " + currentHealth);

        onHealthChanged?.Invoke(); // Вызываем событие

        if (currentHealth <= 0)
        {
            DestroyBase();
        }
    }

    private void DestroyBase()
    {
        isDestroyed = true;
        Debug.Log(gameObject.name + " уничтожена! Игра окончена.");
        gameObject.SetActive(false);
    }
}