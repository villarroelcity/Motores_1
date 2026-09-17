using UnityEngine;
using UnityEngine.Events;

public class GeneratorInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] Material activeMaterial;
    [SerializeField] UnityEvent onActivated = new UnityEvent();
    bool activated;

    public UnityEvent OnActivated => onActivated;
    public bool Activated => activated;
    public string Prompt => activated ? "Generador activo" : "E: restaurar energía del taller";

    public void Interact()
    {
        if (activated) return;
        activated = true;
        Renderer renderer = GetComponentInChildren<Renderer>();
        if (renderer != null && activeMaterial != null)
            renderer.sharedMaterial = activeMaterial;
        onActivated.Invoke();
        Debug.Log("DUSTLINE: generador activado.");
    }
}
