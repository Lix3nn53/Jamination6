using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lix.Core;

public class LootSpawner : MonoBehaviour
{

  // Define a list of weighted items

  [Header("Spawn Settings")]
  [SerializeField] private float _spawnRate = 1f;
  [SerializeField] private float _maxScaleMultiplier = 1.5f;

   private LootPoolManager _lootPool;

  private void Start()
  {
    StartCoroutine(SpawnLoop());
    _lootPool = ServiceLocator.Get<LootPoolManager>();
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
    //LootSpawnerItemSO item = SelectWeightedItem();

    // Spawn the item

    LootType lootType = (LootType)Random.Range(0, System.Enum.GetValues(typeof(LootType)).Length);

    // instantiate zombie
    Loot loot = _lootPool.GetLoot(lootType);
    loot.transform.position = spawnLocation.position;
    loot.transform.rotation = spawnLocation.rotation;
    loot.gameObject.SetActive(true);

    // Random scale
    /*float randomScale = Random.Range(1f, _maxScaleMultiplier);
    Vector3 newScale = loot.transform.localScale;
    newScale *= randomScale;
    loot.transform.localScale = newScale;*/
  }

  private Transform GetRandomSpawnLocation()
  {
    return transform.GetChild(Random.Range(0, transform.childCount));
  }

}
