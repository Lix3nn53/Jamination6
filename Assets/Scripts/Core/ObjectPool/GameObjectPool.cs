using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Lix.Core
{
  public abstract class GameObjectPool : MonoBehaviour
  {
    [SerializeField] private GameObject prefab;
    public enum PoolType
    {
      Stack,
      LinkedList
    }

    public PoolType poolType;

    // Collection checks will throw errors if we try to release an item that is already in the pool.
    public bool collectionChecks = true;
    public int maxPoolSize = 10;

    IObjectPool<GameObject> m_Pool;

    public IObjectPool<GameObject> Pool
    {
      get
      {
        if (m_Pool == null)
        {
          if (poolType == PoolType.Stack)
            m_Pool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, 10, maxPoolSize);
          else
            m_Pool = new LinkedPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, maxPoolSize);
        }
        return m_Pool;
      }
    }

    protected abstract void Awake();

    protected virtual GameObject CreatePooledItem()
    {
      GameObject go = Instantiate(prefab);

      return go;
    }

    // Called when an item is returned to the pool using Release
    protected virtual void OnReturnedToPool(GameObject go)
    {
      go.SetActive(false);
      go.transform.SetParent(transform);
    }

    // Called when an item is taken from the pool using Get
    protected virtual void OnTakeFromPool(GameObject go)
    {
      if (go == null)
      {
        InternalDebug.LogError("ObstaclePool: Trying to take an item from the pool that is null");
        return;
      }

      go.SetActive(true);
    }

    // If the pool capacity is reached then any items returned will be destroyed.
    // We can control what the destroy behavior does, here we destroy the GameObject.
    protected virtual void OnDestroyPoolObject(GameObject go)
    {
      Destroy(go);
    }

    // void OnGUI()
    // {
    //     GUILayout.Label("Pool size: " + Pool.CountInactive);
    //     if (GUILayout.Button("Create Particles"))
    //     {
    //         var amount = Random.Range(1, 10);
    //         for (int i = 0; i < amount; ++i)
    //         {
    //             var ps = Pool.Get();
    //             ps.transform.position = Random.insideUnitSphere * 10;
    //             ps.Play();
    //         }
    //     }
    // }
  }
}