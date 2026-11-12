using UnityEngine;

public class SimpleCamera : MonoBehaviour
{
    public Transform target; // Игрок

    // Смещение камеры относительно игрока
    // X = 0 (по центру), Y = 15 (высоко), Z = -10
    public Vector3 offset = new Vector3(0f, 15f, -10f);

    void LateUpdate()
    {
        if (target == null) return;

        // Камера встает ровно на позицию игрока + смещение. 
        // Никакой плавности (Lerp) — значит, никакой дрожи и укачивания.
        transform.position = target.position + offset;
    }
}