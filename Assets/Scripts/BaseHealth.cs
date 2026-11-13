using UnityEngine;

public class BaseHealth : MonoBehaviour
{
    public float maxHealth = 500f;
    public float currentHealth;
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
        onHealthChanged?.Invoke();

        if (currentHealth <= 0)
        {
            DestroyBase();
        }
    }

    private void DestroyBase()
    {
        isDestroyed = true;
        currentHealth = 0;
        Debug.Log(gameObject.name + " уничтожена!");

        // Вместо выключения объекта (SetActive false), мы просто скрываем его визуал
        // и отключаем коллайдер, чтобы по нему нельзя было бить.
        MeshRenderer[] meshes = GetComponentsInChildren<MeshRenderer>();
        foreach (var mesh in meshes) mesh.enabled = false;

        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;
    }
}