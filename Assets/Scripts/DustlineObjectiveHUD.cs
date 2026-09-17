using TMPro;
using UnityEngine;

public class DustlineObjectiveHUD : MonoBehaviour
{
    [SerializeField] DustlineProgress progress;
    [SerializeField] TMP_Text objectiveText;

    public void Configure(DustlineProgress newProgress, TMP_Text text)
    {
        progress = newProgress;
        objectiveText = text;
    }

    void Update()
    {
        if (progress != null && objectiveText != null)
            objectiveText.text = progress.CurrentObjective;
    }
}
