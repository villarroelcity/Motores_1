using UnityEngine;

public class DustlineProgress : MonoBehaviour
{
    public bool PowerRestored { get; private set; }
    public bool BatteryCollected { get; private set; }
    public bool VehicleRecovered { get; private set; }
    public bool Completed { get; private set; }

    public void MarkPowerRestored()
    {
        if (PowerRestored) return;
        PowerRestored = true;
        Debug.Log("DUSTLINE: energía del taller restaurada.");
    }

    public void MarkBatteryCollected()
    {
        if (BatteryCollected) return;
        BatteryCollected = true;
        Debug.Log("DUSTLINE: batería recuperada.");
    }

    public void MarkVehicleRecovered()
    {
        if (VehicleRecovered) return;
        VehicleRecovered = true;
        Debug.Log("DUSTLINE: vehículo preparado. Portón habilitado.");
    }

    public void MarkCompleted()
    {
        if (Completed) return;
        Completed = true;
        Debug.Log("DUSTLINE: prototipo completado.");
    }

    public string CurrentObjective
    {
        get
        {
            if (Completed) return "PROTOTIPO COMPLETADO";
            if (VehicleRecovered) return "Vehículo listo. Cruzá el portón del taller.";
            if (BatteryCollected) return "Llevá la batería al vehículo e instalala.";
            if (PowerRestored) return "La energía volvió. Recogé la batería generada.";
            return "Explorá el taller. El vehículo necesita energía para volver a funcionar.";
        }
    }
}
