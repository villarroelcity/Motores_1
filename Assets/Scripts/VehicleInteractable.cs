using UnityEngine;
using UnityEngine.Events;

public class VehicleInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] PlayerCargo cargo;
    [SerializeField] DustlineProgress progress;
    [SerializeField] UnityEvent onRecovered = new UnityEvent();
    bool recovered;

    public UnityEvent OnRecovered => onRecovered;
    public bool Recovered => recovered;

    public string Prompt
    {
        get
        {
            if (recovered) return "VEHÍCULO LISTO // RUTA HABILITADA";
            if (cargo != null && cargo.HasBattery) return "E  //  INSTALAR BATERÍA";
            return "SIN ENERGÍA // REQUIERE BATERÍA";
        }
    }

    public void Configure(PlayerCargo newCargo, DustlineProgress newProgress)
    {
        cargo = newCargo;
        progress = newProgress;
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
        onRecovered.Invoke();
    }
}
