using UnityEngine;
using UnityEngine.SceneManagement; // Нужно для загрузки сцены

public class RestartButton : MonoBehaviour
{
    public void RestartGame()
    {
        // Возвращаем время в норму (так как при конце игры мы его остановили)
        Time.timeScale = 1f;

        // Перезагружаем текущую сцену
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}