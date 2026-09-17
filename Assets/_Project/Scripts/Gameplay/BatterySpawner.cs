using UnityEngine;

public class BatterySpawner : MonoBehaviour
{
    [SerializeField] GameObject batteryPrefab;
    [SerializeField] Transform spawnPoint;
    GameObject spawnedBattery;

    public void Spawn()
    {
        if (spawnedBattery != null || batteryPrefab == null || spawnPoint == null) return;
        spawnedBattery = Instantiate(batteryPrefab, spawnPoint.position, spawnPoint.rotation);
        Debug.Log("DUSTLINE: batería instanciada.");
    }
}
