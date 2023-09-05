using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lix.Core;

public class ZombieFactory : FactoryMono<Zombie>
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

  public override Zombie Create()
  {
    return Get(ZombieType.Attacker, Vector3.zero, Quaternion.identity);
  }

  public Zombie Create(ZombieType type)
  {
    return Get(type, Vector3.zero, Quaternion.identity);
  }
}
