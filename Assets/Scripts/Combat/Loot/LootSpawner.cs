using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSpawner : MonoBehaviour
{

  // Define a list of weighted items
  [SerializeField] private List<LootSpawnerItemSO> _weightedList = new List<LootSpawnerItemSO>();

  [Header("Spawn Settings")]
  [SerializeField] private float _spawnRate = 1f;
  [SerializeField] private float _maxScaleMultiplier = 1.5f;

   private LootPoolManager _lootPool;

  private void Start()
  {
    StartCoroutine(SpawnLoop());
  }

  private IEnumerator SpawnLoop()
  {
    yield return new WaitForSeconds(3f);

    while (true)
    {
      Spawn();
      yield return new WaitForSeconds(_spawnRate);
    }
  }

  public void Spawn()
  {
    // Get a random spawn location
    Transform spawnLocation = GetRandomSpawnLocation();

    // Select a weighted item at random
    LootSpawnerItemSO item = SelectWeightedItem();

    // Spawn the item

    LootType lootType = (LootType)Random.Range(0, System.Enum.GetValues(typeof(LootType)).Length);

    // instantiate zombie
    Loot loot = _lootPool.GetLoot(lootType);
    loot.transform.position = transform.position;
    loot.transform.rotation = transform.rotation;
    loot.gameObject.SetActive(true);

    // Random scale
    float randomScale = Random.Range(1f, _maxScaleMultiplier);
    Vector3 newScale = loot.transform.localScale;
    newScale *= randomScale;
    loot.transform.localScale = newScale;
  }

  private Transform GetRandomSpawnLocation()
  {
    return transform.GetChild(Random.Range(0, transform.childCount));
  }

  // Select a weighted item at random
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
