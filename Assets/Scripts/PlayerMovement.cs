using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10f;
    private Rigidbody rb;
    private Vector3 movementInput;

    // Сюда мы перетащим камеру, чтобы игрок понимал, куда двигаться
    public Transform cameraTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        // 1. Берем направление "вперед" и "вправо" относительно камеры
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // 2. Убираем наклон по вертикали (Y), чтобы игрок не пытался залететь в небо или уйти под землю
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        // 3. Формируем вектор движения (W = вперед по экрану, D = вправо по экрану)
        movementInput = (forward * moveZ + right * moveX).normalized;
    }

    void FixedUpdate()
    {
        // Двигаем игрока
        Vector3 targetVelocity = movementInput * moveSpeed;
        targetVelocity.y = rb.linearVelocity.y; // Оставляем гравитацию

        rb.linearVelocity = targetVelocity;

        // Поворачиваем капсулу в сторону движения
        if (movementInput != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movementInput);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.fixedDeltaTime);
        }
    }
}