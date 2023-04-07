using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lix.Core
{
  public static class PoolManager
  {
    public static Dictionary<string, GameObjectPool> PoolDict = new Dictionary<string, GameObjectPool>();

    public static GameObjectPool Get(string poolName)
    {
      return PoolDict[poolName];
    }

    public static void Add(string poolName, GameObjectPool pool)
    {
      if (PoolDict.ContainsKey(poolName))
      {
        InternalDebug.LogWarning("PoolManager: Pool with name " + poolName + " already exists. Overriding.");
        PoolDict[poolName] = pool;
        return;
      }

      PoolDict.Add(poolName, pool);
    }
  }
}