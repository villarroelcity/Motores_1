using UnityEngine;
using UnityEngine.Events;

public class DustlineProgress : MonoBehaviour
{
    public bool PowerRestored { get; private set; }
    public bool BatteryCollected { get; private set; }
    public bool VehicleRecovered { get; private set; }
    public bool Completed { get; private set; }

    [SerializeField] UnityEvent onPowerRestored = new UnityEvent();
    [SerializeField] UnityEvent onBatteryCollected = new UnityEvent();
    [SerializeField] UnityEvent onVehicleRecovered = new UnityEvent();
    [SerializeField] UnityEvent onCompleted = new UnityEvent();

    public UnityEvent OnPowerRestored => onPowerRestored;
    public UnityEvent OnBatteryCollected => onBatteryCollected;
    public UnityEvent OnVehicleRecovered => onVehicleRecovered;
    public UnityEvent OnCompleted => onCompleted;

    public void MarkPowerRestored()
    {
        if (PowerRestored) return;
        PowerRestored = true;
        onPowerRestored.Invoke();
        Debug.Log("DUSTLINE: energía del taller restaurada.");
    }

    public void MarkBatteryCollected()
    {
        if (BatteryCollected) return;
        BatteryCollected = true;
        onBatteryCollected.Invoke();
        Debug.Log("DUSTLINE: batería recuperada.");
    }

    public void MarkVehicleRecovered()
    {
        if (VehicleRecovered) return;
        VehicleRecovered = true;
        onVehicleRecovered.Invoke();
        Debug.Log("DUSTLINE: vehículo preparado. Ruta de salida habilitada.");
    }

    public void MarkCompleted()
    {
        if (Completed) return;
        Completed = true;
        onCompleted.Invoke();
        Debug.Log("DUSTLINE: prototipo completado.");
    }

    public string CurrentObjective
    {
        get
        {
            if (Completed) return "RUTA DESBLOQUEADA — PROTOTIPO COMPLETADO";
            if (VehicleRecovered) return "VEHÍCULO RECUPERADO — cruzá el portón y alcanzá la ruta.";
            if (BatteryCollected) return "BATERÍA RECUPERADA — regresá al vehículo e instalala.";
            if (PowerRestored) return "ENERGÍA RESTAURADA — buscá el módulo de batería en PARTS.";
            return "ESTACIÓN 01 — inspeccioná el vehículo y restaurá la energía del taller.";
        }
    }
}
