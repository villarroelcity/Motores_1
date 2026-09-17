using UnityEngine;

public class VehicleRecoveryVisual : MonoBehaviour
{
    [SerializeField] Light[] headlights;
    [SerializeField] Renderer statusRenderer;
    [SerializeField] Material recoveredMaterial;

    public void Configure(Light[] newHeadlights, Renderer newStatusRenderer, Material material)
    {
        headlights = newHeadlights;
        statusRenderer = newStatusRenderer;
        recoveredMaterial = material;
    }

    public void Recover()
    {
        if (headlights != null)
            foreach (Light l in headlights)
                if (l != null) l.enabled = true;
        if (statusRenderer != null && recoveredMaterial != null)
            statusRenderer.sharedMaterial = recoveredMaterial;
        Debug.Log("DUSTLINE: feedback visual de vehículo recuperado activado.");
    }
}
