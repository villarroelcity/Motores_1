using UnityEngine;

public class WorkshopPowerSystem : MonoBehaviour
{
    [SerializeField] Light[] poweredLights;
    [SerializeField] Renderer[] statusRenderers;
    [SerializeField] Material onlineMaterial;

    public void Configure(Light[] lights, Renderer[] renderers, Material material)
    {
        poweredLights = lights;
        statusRenderers = renderers;
        onlineMaterial = material;
    }

    public void RestorePower()
    {
        if (poweredLights != null)
            foreach (Light l in poweredLights)
                if (l != null) l.enabled = true;

        if (statusRenderers != null && onlineMaterial != null)
            foreach (Renderer r in statusRenderers)
                if (r != null) r.sharedMaterial = onlineMaterial;

        Debug.Log("DUSTLINE: iluminación y sistemas visuales del taller encendidos.");
    }
}
