using UnityEngine;

namespace EnElCamino
{
    public class CheckPoint : MonoBehaviour
    {
        [SerializeField] private Juego gameFlow;

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponentInParent<AutoJugador>() == null || gameFlow == null) return;

            if (name == "Persecucion")
                gameFlow.NotifyPursuitTrigger();
            else if (name == "SalidaAlCamino")
            {
                gameFlow.ReachRoad();
            }
            else
                gameFlow.TryFinish();
        }
    }
}
