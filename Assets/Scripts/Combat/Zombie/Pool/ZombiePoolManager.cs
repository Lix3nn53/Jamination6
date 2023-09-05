using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lix.Core;

public class ZombiePoolManager : MonoBehaviour
{
  [SerializeField] private SerializableDictionary<ZombieType, ZombiePool> _pools = new SerializableDictionary<ZombieType, ZombiePool>();

  public Zombie GetZombie(ZombieType type)
  {
    if (!_pools.ContainsKey(type))
    {
      Debug.LogError("No pool for zombie type " + type);
      return null;
    }

    return _pools[type].Pool.Get();
  }
}
