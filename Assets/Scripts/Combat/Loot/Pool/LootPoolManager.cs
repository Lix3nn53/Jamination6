using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lix.Core;

public class LootPoolManager : MonoBehaviour
{
  [SerializeField] private SerializableDictionary<LootType, LootPool> _pools = new SerializableDictionary<LootType, LootPool>();

  public Loot GetLoot(LootType type)
  {
    if (!_pools.ContainsKey(type))
    {
      Debug.LogError("No pool for zombie type " + type);
      return null;
    }

    return _pools[type].Pool.Get();
  }
}
