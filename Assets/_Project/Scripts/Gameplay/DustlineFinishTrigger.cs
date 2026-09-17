using UnityEngine;

public class DustlineFinishTrigger : MonoBehaviour
{
    [SerializeField] DustlineProgress progress;

    void OnTriggerEnter(Collider other)
    {
        if (progress == null || progress.Completed || !progress.VehicleRecovered) return;
        if (other.GetComponent<PlayerMovement>() == null) return;
        progress.MarkCompleted();
    }
}
