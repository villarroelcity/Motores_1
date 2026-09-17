using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DustlinePlayerInteraction : MonoBehaviour
{
    [SerializeField] Camera view;
    [SerializeField] InputActionAsset inputActions;
    [SerializeField] float reach = 3.2f;
    [SerializeField] TMP_Text prompt;

    InputActionAsset runtimeActions;
    InputAction interact;

    public void Configure(Camera newView, InputActionAsset actions, TMP_Text newPrompt, float newReach = 3.2f)
    {
        view = newView;
        inputActions = actions;
        prompt = newPrompt;
        reach = newReach;
    }

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
        if (Physics.Raycast(ray, out RaycastHit hit, 15f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore)
            && Vector3.Distance(transform.position, hit.collider.ClosestPoint(transform.position)) <= reach)
            return hit.collider.GetComponentInParent<IInteractable>();
        return null;
    }

    void Update()
    {
        IInteractable target = Cursor.lockState == CursorLockMode.Locked ? FindTarget() : null;
        if (prompt != null)
        {
            if (Cursor.lockState != CursorLockMode.Locked) prompt.text = "CLIC PARA CAPTURAR EL MOUSE";
            else prompt.text = target != null ? target.Prompt : string.Empty;
        }

        bool pressed = interact != null
            ? interact.WasPressedThisFrame()
            : Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame;

        if (target != null && pressed) target.Interact();
    }
}
