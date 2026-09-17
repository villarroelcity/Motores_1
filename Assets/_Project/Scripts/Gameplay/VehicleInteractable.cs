using UnityEngine;
using UnityEngine.Events;

public class VehicleInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] PlayerCargo cargo;
    [SerializeField] DustlineProgress progress;
    [SerializeField] Material recoveredMaterial;
    [SerializeField] UnityEvent onRecovered = new UnityEvent();
    bool recovered;

    public UnityEvent OnRecovered => onRecovered;
    public bool Recovered => recovered;

    public string Prompt
    {
        get
        {
            if (recovered) return "Vehículo preparado para salir";
            if (cargo != null && cargo.HasBattery) return "E: instalar batería";
            return "Vehículo sin energía. Falta una batería.";
        }
    }

    public void Interact()
    {
        if (recovered) return;
        if (cargo == null || !cargo.TryConsumeBattery())
        {
            Debug.Log("DUSTLINE: el vehículo necesita una batería.");
            return;
        }

        recovered = true;
        progress?.MarkVehicleRecovered();
        Renderer renderer = GetComponentInChildren<Renderer>();
        if (renderer != null && recoveredMaterial != null)
            renderer.sharedMaterial = recoveredMaterial;
        onRecovered.Invoke();
    }
}
