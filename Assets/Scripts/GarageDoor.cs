using UnityEngine;

public class GarageDoor : MonoBehaviour
{
    [SerializeField] Vector3 openOffset = new Vector3(0f, 5.5f, 0f);
    [SerializeField] float speed = 3.5f;
    Vector3 closedPosition;
    Vector3 targetPosition;

    public bool IsOpen { get; private set; }

    void Awake()
    {
        closedPosition = transform.position;
        targetPosition = closedPosition;
    }

    public void Configure(Vector3 offset, float movementSpeed)
    {
        openOffset = offset;
        speed = movementSpeed;
    }

    public void Open()
    {
        if (IsOpen) return;
        IsOpen = true;
        targetPosition = closedPosition + openOffset;
        Debug.Log("DUSTLINE: portón de ruta abriéndose.");
    }

    void Update()
    {
        if (!IsOpen) return;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }
}
