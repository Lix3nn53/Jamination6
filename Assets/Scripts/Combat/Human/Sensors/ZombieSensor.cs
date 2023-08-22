using UnityEngine;

namespace LlamAcademy.Sensors
{
    [RequireComponent(typeof(SphereCollider))]
    public class ZombieSensor : MonoBehaviour
    {
        public delegate void PlayerEnterEvent(Player player);
        public delegate void PlayerExitEvent(Player player);
        public event PlayerEnterEvent OnPlayerEnter;
        public event PlayerExitEvent OnPlayerExit;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player player))
            {
                OnPlayerEnter?.Invoke(player);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Player player))
            {
                OnPlayerExit?.Invoke(player);
            }
        }
    }
}
