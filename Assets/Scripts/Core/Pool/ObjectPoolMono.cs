using UnityEngine.Pool;
using UnityEngine;

namespace Lix.Core
{
  public abstract class ObjectPoolMono<T> : MonoBehaviour, IObjectPool<T> where T : class
  {
    [SerializeField] private bool collectionCheck = true;
    [SerializeField] private int defaultCapacity = 10;
    [SerializeField] private int maxSize = 20;
    [SerializeField] private ObjectPool<T> pool;

    public ObjectPool<T> Pool => pool;

    private void Awake()
    {
      pool = new ObjectPool<T>(CreatePooledObject, OnTakeFromPool, OnReturnToPool, OnDestroyObject, collectionCheck, defaultCapacity, maxSize);
      InternalDebug.Log("Created MonoBehaviour pool for " + typeof(T) + " with " + pool.CountAll + " objects");

      Prewarm();
    }

    private void Prewarm()
    {
      T[] instances = new T[defaultCapacity];
      for (int i = 0; i < defaultCapacity; i++)
      {
        instances[i] = Pool.Get();
      }
      for (int i = 0; i < defaultCapacity; i++)
      {
        Pool.Release(instances[i]);
      }
    }

    public abstract T CreatePooledObject();

    public abstract void OnTakeFromPool(T Instance);

    public abstract void OnReturnToPool(T Instance);

    public abstract void OnDestroyObject(T Instance);
  }
}