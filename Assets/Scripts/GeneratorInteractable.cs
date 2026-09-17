using UnityEngine;
using UnityEngine.Events;

public class GeneratorInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] Material activeMaterial;
    [SerializeField] DustlineProgress progress;
    [SerializeField] UnityEvent onActivated = new UnityEvent();
    bool activated;

    public UnityEvent OnActivated => onActivated;
    public bool Activated => activated;
    public string Prompt => activated ? "GENERADOR ONLINE" : "E  //  RESTAURAR ENERGÍA";

    public void Configure(DustlineProgress newProgress, Material newActiveMaterial)
    {
        progress = newProgress;
        activeMaterial = newActiveMaterial;
    }

    public void Interact()
    {
        if (activated) return;
        activated = true;

        Renderer renderer = GetComponentInChildren<Renderer>();
        if (renderer != null && activeMaterial != null)
            renderer.sharedMaterial = activeMaterial;

        progress?.MarkPowerRestored();
        onActivated.Invoke();
        Debug.Log("DUSTLINE: generador activado mediante interacción.");
    }
}
