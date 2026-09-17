using UnityEngine;

public class BatteryPickup : MonoBehaviour, IInteractable
{
    [SerializeField] PlayerCargo cargo;
    [SerializeField] DustlineProgress progress;
    bool collected;

    public string Prompt => collected ? string.Empty : "E  //  RECOGER BATERÍA";

    public void Configure(PlayerCargo newCargo, DustlineProgress newProgress)
    {
        cargo = newCargo;
        progress = newProgress;
    }

    public void Interact()
    {
        if (collected) return;
        if (cargo == null) cargo = FindFirstObjectByType<PlayerCargo>();
        if (progress == null) progress = FindFirstObjectByType<DustlineProgress>();
        if (cargo == null || !cargo.TryCollectBattery()) return;

        collected = true;
        progress?.MarkBatteryCollected();
        Destroy(gameObject);
    }
}
