using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DustlinePlayerInteraction : MonoBehaviour
{
    [SerializeField] Camera view;
    [SerializeField] InputActionAsset inputActions;
    [SerializeField] float reach = 3f;
    [SerializeField] TMP_Text prompt;
    [SerializeField] DustlineProgress progress;

    InputActionAsset runtimeActions;
    InputAction interact;

    void Awake()
    {
        if (inputActions == null) return;
        runtimeActions = Instantiate(inputActions);
        interact = runtimeActions.FindAction("Player/Interact", true);
    }

    void OnEnable() => interact?.Enable();
    void OnDisable() => interact?.Disable();
    void OnDestroy() { if (runtimeActions) Destroy(runtimeActions); }

    public IInteractable FindTarget()
    {
        if (view == null) return null;
        Ray ray = view.ViewportPointToRay(new Vector3(.5f, .5f));
        if (Physics.Raycast(ray, out RaycastHit hit, 12f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore)
            && Vector3.Distance(transform.position, hit.collider.ClosestPoint(transform.position)) <= reach)
            return hit.collider.GetComponentInParent<IInteractable>();
        return null;
    }

    void Update()
    {
        if (prompt == null) return;

        if (Cursor.lockState != CursorLockMode.Locked)
        {
            prompt.text = "Clic para jugar";
            return;
        }

        IInteractable target = FindTarget();
        prompt.text = target != null ? target.Prompt : progress != null ? progress.CurrentObjective : string.Empty;

        bool pressed = interact != null
            ? interact.WasPressedThisFrame()
            : Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame;

        if (target != null && pressed)
            target.Interact();
    }
}
