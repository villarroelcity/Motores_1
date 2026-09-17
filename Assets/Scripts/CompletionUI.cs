using TMPro;
using UnityEngine;

public class CompletionUI : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] TMP_Text title;

    public void Configure(GameObject newPanel, TMP_Text newTitle)
    {
        panel = newPanel;
        title = newTitle;
        if (panel != null) panel.SetActive(false);
    }

    public void Show()
    {
        if (title != null)
            title.text = "RUTA DESBLOQUEADA\n<color=#E69A42>PARCIAL 1 COMPLETADO</color>\n<size=60%>Próxima etapa: conducción y recuperación en ruta</size>";
        if (panel != null) panel.SetActive(true);
    }
}
