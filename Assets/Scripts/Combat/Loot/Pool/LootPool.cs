using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lix.Core;

public class LootPool : ObjectPoolMono<Loot>
{
  [SerializeField] private LootFactory _factory;
  [SerializeField] private List<LootSpawnerItemSO> _weightedList = new List<LootSpawnerItemSO>();

  public override Loot CreatePooledObject()
  {
    LootSpawnerItemSO item = SelectWeightedItem();
    Loot instance = _factory.Create(item.lootType);
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

  private LootSpawnerItemSO SelectWeightedItem()
  {
    // Calculate the total weight of all items
    float totalWeight = 0;
    foreach (LootSpawnerItemSO item in _weightedList)
    {
      totalWeight += item.Weight;
    }

    // Generate a random number between 0 and the total weight
    float randomWeight = Random.Range(0f, totalWeight);

    // Loop through the items and subtract their weight from the random number
    // until we find the selected item
    foreach (LootSpawnerItemSO item in _weightedList)
    {
      if (randomWeight < item.Weight)
      {
        return item;
      }
      randomWeight -= item.Weight;
    }

    // This should never happen, but just in case
    return null;
  }
}
