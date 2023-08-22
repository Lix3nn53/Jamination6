using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ZombieSensor : MonoBehaviour
{
    public delegate void ZombieEnterEvent(GameObject zombie);
    public delegate void ZombieExitEvent(GameObject zombie);
    public event ZombieEnterEvent OnZombieEnter;
    public event ZombieExitEvent OnZombieExit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Zombie zombie))
        {
            OnZombieEnter?.Invoke(zombie.gameObject);
        }
        else if (other.TryGetComponent(out Player player))
        {
            OnZombieEnter?.Invoke(player.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Zombie zombie))
        {
            OnZombieExit?.Invoke(zombie.gameObject);
        }
        else if (other.TryGetComponent(out Player player))
        {
            OnZombieEnter?.Invoke(player.gameObject);
        }
    }
}
