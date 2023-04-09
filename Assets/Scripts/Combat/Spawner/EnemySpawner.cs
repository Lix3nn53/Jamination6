using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

  // Define a list of weighted items
  [SerializeField] private List<EnemySpawnerItemSO> weightedList = new List<EnemySpawnerItemSO>();

  [Header("Spawn Settings")]
  [SerializeField] private float spawnRate = 1f;

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
      yield return new WaitForSeconds(spawnRate);
    }
  }

  public void Spawn()
  {
    // Get a random spawn location
    Transform spawnLocation = GetRandomSpawnLocation();

    // Select a weighted item at random
    EnemySpawnerItemSO item = SelectWeightedItem();

    // Spawn the item
    Instantiate(item.Prefab, spawnLocation.position, spawnLocation.rotation);
  }

  private Transform GetRandomSpawnLocation()
  {
    return transform.GetChild(Random.Range(0, transform.childCount));
  }

  // Select a weighted item at random
  private EnemySpawnerItemSO SelectWeightedItem()
  {
    // Calculate the total weight of all items
    float totalWeight = 0;
    foreach (EnemySpawnerItemSO item in weightedList)
    {
      totalWeight += item.Weight;
    }

    // Generate a random number between 0 and the total weight
    float randomWeight = Random.Range(0f, totalWeight);

    // Loop through the items and subtract their weight from the random number
    // until we find the selected item
    foreach (EnemySpawnerItemSO item in weightedList)
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
