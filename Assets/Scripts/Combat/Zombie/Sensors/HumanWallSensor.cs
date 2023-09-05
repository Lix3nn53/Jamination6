using UnityEngine;

namespace LlamAcademy.Sensors
{
    [RequireComponent(typeof(SphereCollider))]
    public class HumanWallSensor : MonoBehaviour
    {
        public delegate void EnterEvent(GameObject human);
        public delegate void ExitEvent(GameObject human);
        public event EnterEvent OnEnter;
        public event ExitEvent OnExit;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out HumanWall human))
            {
                OnEnter?.Invoke(human.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out HumanWall human))
            {
                OnExit?.Invoke(human.gameObject);
            }
        }
    }
}
