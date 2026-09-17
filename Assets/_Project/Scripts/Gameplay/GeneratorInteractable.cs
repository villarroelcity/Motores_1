using UnityEngine;
using UnityEngine.Events;

public class GeneratorInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] Material activeMaterial;
    [SerializeField] BatterySpawner spawner;
    [SerializeField] DustlineProgress progress;
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

        // Conexiones directas para que el prototipo funcione apenas se abre la escena.
        // El UnityEvent se mantiene para demostrar el sistema de eventos visto en Clase 7.
        spawner?.Spawn();
        progress?.MarkPowerRestored();
        onActivated.Invoke();

        Debug.Log("DUSTLINE: generador activado.");
    }
}
