using UnityEngine;

public class DustlineFinishTrigger : MonoBehaviour
{
    [SerializeField] DustlineProgress progress;
    [SerializeField] CompletionUI completionUI;

    public void Configure(DustlineProgress newProgress, CompletionUI newCompletionUI)
    {
        progress = newProgress;
        completionUI = newCompletionUI;
    }

    void OnTriggerEnter(Collider other)
    {
        if (progress == null || progress.Completed || !progress.VehicleRecovered) return;
        if (other.GetComponent<PlayerMovement>() == null) return;
        progress.MarkCompleted();
        completionUI?.Show();
    }
}
