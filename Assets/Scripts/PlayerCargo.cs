using UnityEngine;

public class PlayerCargo : MonoBehaviour
{
    public bool HasBattery { get; private set; }

    public bool TryCollectBattery()
    {
        if (HasBattery) return false;
        HasBattery = true;
        return true;
    }

    public bool TryConsumeBattery()
    {
        if (!HasBattery) return false;
        HasBattery = false;
        return true;
    }
}
