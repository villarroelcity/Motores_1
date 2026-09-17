using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] InputActionAsset inputActions;
    [SerializeField] Transform cameraTransform;
    [SerializeField] float speed = 4.5f;
    [SerializeField] float rotationSpeed = 540f;
    [SerializeField] float gravity = -20f;
    CharacterController controller;
    InputActionAsset runtimeActions;
    InputAction move;
    Vector3 startPosition;
    float verticalSpeed;

    public void Configure(InputActionAsset actions, Transform cam)
    {
        inputActions = actions;
        cameraTransform = cam;
    }

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        startPosition = transform.position;
        if (inputActions != null)
        {
            runtimeActions = Instantiate(inputActions);
            move = runtimeActions.FindAction("Player/Move", true);
        }
    }
    void OnEnable() { move?.Enable(); }
    void OnDisable() { move?.Disable(); }
    void OnDestroy() { if (runtimeActions) Destroy(runtimeActions); }

    void Update()
    {
        if (controller == null || cameraTransform == null) return;
        Vector2 input = Cursor.lockState == CursorLockMode.Locked && move != null ? move.ReadValue<Vector2>() : Vector2.zero;
        Vector3 forward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
        Vector3 right = Vector3.Cross(Vector3.up, forward);
        Vector3 direction = Vector3.ClampMagnitude(forward * input.y + right * input.x, 1f);
        if (controller.isGrounded && verticalSpeed < 0) verticalSpeed = -2f;
        verticalSpeed += gravity * Time.deltaTime;
        controller.Move((direction * speed + Vector3.up * verticalSpeed) * Time.deltaTime);
        if (direction.sqrMagnitude > .001f)
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
        if (transform.position.y < -10f) Respawn();
    }

    public void Respawn()
    {
        controller.enabled = false;
        transform.position = startPosition;
        verticalSpeed = 0;
        controller.enabled = true;
    }
}
