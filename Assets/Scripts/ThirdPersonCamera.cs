using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float distance = 5.2f;
    [SerializeField] float height = 1.45f;
    [SerializeField] float sensitivity = .12f;
    [SerializeField] LayerMask obstacleMask = 1;
    float yaw;
    float pitch = 18f;

    public void Configure(Transform newTarget) => target = newTarget;

    void OnEnable() { Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false; }
    void OnDisable() { Cursor.lockState = CursorLockMode.None; Cursor.visible = true; }
    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        { Cursor.lockState = CursorLockMode.None; Cursor.visible = true; }
        else if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        { Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false; }
    }
    void LateUpdate()
    {
        if (!target) return;
        if (Mouse.current != null && Cursor.lockState == CursorLockMode.Locked)
        {
            Vector2 look = Mouse.current.delta.ReadValue();
            yaw += look.x * sensitivity;
            pitch = Mathf.Clamp(pitch - look.y * sensitivity, -15f, 62f);
        }
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 pivot = target.position + Vector3.up * height;
        Vector3 backward = rotation * Vector3.back;
        float safeDistance = distance;
        if (Physics.SphereCast(pivot, .22f, backward, out RaycastHit hit, distance, obstacleMask, QueryTriggerInteraction.Ignore))
            safeDistance = Mathf.Max(.08f, hit.distance - .08f);
        transform.SetPositionAndRotation(pivot + backward * safeDistance, rotation);
    }
}
