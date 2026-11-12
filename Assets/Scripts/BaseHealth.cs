using UnityEngine;

public class BaseHealth : MonoBehaviour
{
    public float maxHealth = 500f;
    private float currentHealth;
    private bool isDestroyed = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (isDestroyed) return;

        currentHealth -= amount;
        Debug.Log("База получила урон! Осталось HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            DestroyBase();
        }
    }

    private void DestroyBase()
    {
        isDestroyed = true;
        Debug.Log("База уничтожена! Игра окончена.");
        // Тут потом добавим экран поражения
        gameObject.SetActive(false); // Визуально скрываем базу
    }
}