using UnityEngine;

namespace LlamAcademy.Sensors
{
    [RequireComponent(typeof(SphereCollider))]
    public class HumanSensor : MonoBehaviour
    {
        public delegate void HumanEnterEvent(GameObject human);
        public delegate void HumanExitEvent(GameObject human);
        public event HumanEnterEvent OnHumanEnter;
        public event HumanExitEvent OnHumanExit;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Human human))
            {
                OnHumanEnter?.Invoke(human.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Human human))
            {
                OnHumanExit?.Invoke(human.gameObject);
            }
        }
    }
}
