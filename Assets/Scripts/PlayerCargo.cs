using UnityEngine;

public class PlayerCargo : MonoBehaviour
{
    [SerializeField] Transform carrySocket;
    [SerializeField] Material carryMaterial;
    GameObject carryVisual;

    public bool HasBattery { get; private set; }

    public void Configure(Transform socket, Material material)
    {
        carrySocket = socket;
        carryMaterial = material;
    }

    public bool TryCollectBattery()
    {
        if (HasBattery) return false;
        HasBattery = true;
        CreateCarryVisual();
        return true;
    }

    public bool TryConsumeBattery()
    {
        if (!HasBattery) return false;
        HasBattery = false;
        if (carryVisual != null) Destroy(carryVisual);
        return true;
    }

    void CreateCarryVisual()
    {
        if (carrySocket == null || carryVisual != null) return;
        carryVisual = GameObject.CreatePrimitive(PrimitiveType.Cube);
        carryVisual.name = "CarriedBattery_Visual";
        carryVisual.transform.SetParent(carrySocket, false);
        carryVisual.transform.localPosition = Vector3.zero;
        carryVisual.transform.localRotation = Quaternion.Euler(0f, 0f, 8f);
        carryVisual.transform.localScale = new Vector3(.5f, .32f, .28f);
        Collider col = carryVisual.GetComponent<Collider>();
        if (col != null) Destroy(col);
        Renderer r = carryVisual.GetComponent<Renderer>();
        if (r != null && carryMaterial != null) r.sharedMaterial = carryMaterial;
    }
}
