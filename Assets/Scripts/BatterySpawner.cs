using UnityEngine;

public class BatterySpawner : MonoBehaviour
{
    [SerializeField] GameObject batteryPrefab;
    [SerializeField] Transform spawnPoint;
    GameObject spawnedBattery;

    public void Configure(GameObject prefab, Transform point)
    {
        batteryPrefab = prefab;
        spawnPoint = point;
    }

    public void Spawn()
    {
        if (spawnedBattery != null || batteryPrefab == null || spawnPoint == null) return;
        spawnedBattery = Instantiate(batteryPrefab, spawnPoint.position, spawnPoint.rotation);
        spawnedBattery.name = "Battery_Module_RUNTIME";
        Debug.Log("DUSTLINE: módulo de batería instanciado desde prefab.");
    }
}
