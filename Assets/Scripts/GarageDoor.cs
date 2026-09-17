using UnityEngine;

public class GarageDoor : MonoBehaviour
{
    [SerializeField] Vector3 openOffset = new Vector3(0f, 4f, 0f);
    public bool IsOpen { get; private set; }

    public void Open()
    {
        if (IsOpen) return;
        transform.position += openOffset;
        IsOpen = true;
        Debug.Log("DUSTLINE: portón abierto.");
    }
}
