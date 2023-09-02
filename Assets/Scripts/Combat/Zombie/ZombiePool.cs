using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePool : MonoBehaviour
{
    [SerializeField] Zombie _zombiePrefab;
    [SerializeField] ZombieHealer _zombieHealerPrefab;
    [SerializeField] ZombieTank _zombieTankPrefab;
    [SerializeField] ZombieCollector _zombieCollectorPrefab;

    public Zombie Get(ZombieType type, Vector3 position, Quaternion rotation)
    {
        switch (type)
        {
            case ZombieType.Healer:
                return Instantiate(_zombieHealerPrefab, position, rotation);
            case ZombieType.Tank:
                return Instantiate(_zombieTankPrefab, position, rotation);
            case ZombieType.Collector:
                return Instantiate(_zombieCollectorPrefab, position, rotation);
            default:
                return Instantiate(_zombiePrefab, position, rotation);
        }
    }
}
