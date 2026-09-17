using UnityEngine;

public class CollisionFeedback : MonoBehaviour
{
    bool reported;

    void OnCollisionEnter(Collision collision)
    {
        if (reported) return;
        reported = true;
        Debug.Log($"DUSTLINE: colisión física entre {name} y {collision.gameObject.name}.");
    }
}
