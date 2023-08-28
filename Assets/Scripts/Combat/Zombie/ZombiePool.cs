using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePool : MonoBehaviour
{
    [SerializeField] Zombie _zombiePrefab;
    [SerializeField] ZombieHealer _zombieHealerPrefab;

    public Zombie Get(ZombieType type, Vector3 position, Quaternion rotation)
    {
        switch (type)
        {
            case ZombieType.Healer:
                return Instantiate(_zombieHealerPrefab, position, rotation);
            default:
                return Instantiate(_zombiePrefab, position, rotation);
        }
    }
}
