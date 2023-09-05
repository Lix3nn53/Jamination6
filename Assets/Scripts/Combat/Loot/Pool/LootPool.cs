using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lix.Core;

public class LootPool : ObjectPoolMono<Loot>
{
  [SerializeField] private LootFactory _factory;
  [SerializeField] private LootType _lootType;

  public override Loot CreatePooledObject()
  {
    Loot instance = _factory.Create(_lootType);
    instance.gameObject.SetActive(false);

    return instance;
  }

  public override void OnTakeFromPool(Loot instance)
  {
    // Instance.gameObject.SetActive(true);
    instance.LootPool = this;
  }

  public override void OnReturnToPool(Loot instance)
  {
    instance.gameObject.SetActive(false);
  }

  public override void OnDestroyObject(Loot instance)
  {
    Destroy(instance.gameObject);
  }
}
