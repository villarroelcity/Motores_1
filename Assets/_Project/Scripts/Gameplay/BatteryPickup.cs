using UnityEngine;

public class BatteryPickup : MonoBehaviour, IInteractable
{
    bool collected;
    public string Prompt => collected ? "" : "E: recoger batería";

    public void Interact()
    {
        if (collected) return;
        PlayerCargo cargo = FindFirstObjectByType<PlayerCargo>();
        if (cargo == null || !cargo.TryCollectBattery()) return;

        collected = true;
        DustlineProgress progress = FindFirstObjectByType<DustlineProgress>();
        progress?.MarkBatteryCollected();
        Destroy(gameObject);
    }
}
