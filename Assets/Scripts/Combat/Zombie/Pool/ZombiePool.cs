using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lix.Core;

public class ZombiePool : ObjectPoolMono<Zombie>
{
  [SerializeField] private ZombieFactory _factory;
  [SerializeField] private ZombieType _zombieType;

  public override Zombie CreatePooledObject()
  {
    Zombie instance = _factory.Create(_zombieType);
    instance.gameObject.SetActive(false);

    return instance;
  }

  public override void OnTakeFromPool(Zombie instance)
  {
    // Instance.gameObject.SetActive(true);
    instance.ZombiePool = this;
  }

  public override void OnReturnToPool(Zombie instance)
  {
    instance.gameObject.SetActive(false);
  }

  public override void OnDestroyObject(Zombie instance)
  {
    Destroy(instance.gameObject);
  }
}
