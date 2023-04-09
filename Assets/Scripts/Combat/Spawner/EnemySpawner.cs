using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

  // Define a list of weighted items
  [SerializeField] private List<EnemySpawnerItemSO> _weightedList = new List<EnemySpawnerItemSO>();

  [Header("Spawn Settings")]
  [SerializeField] private float _spawnRate = 1f;
  [SerializeField] private float _maxScaleMultiplier = 1.5f;

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
    EnemySpawnerItemSO item = SelectWeightedItem();

    // Spawn the item
    GameObject spawned = Instantiate(item.Prefab, spawnLocation.position, spawnLocation.rotation);

    // Random scale
    float randomScale = Random.Range(1f, _maxScaleMultiplier);
    Vector3 newScale = spawned.transform.localScale;
    newScale *= randomScale;
    spawned.transform.localScale = newScale;
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
    foreach (EnemySpawnerItemSO item in _weightedList)
    {
      totalWeight += item.Weight;
    }

    // Generate a random number between 0 and the total weight
    float randomWeight = Random.Range(0f, totalWeight);

    // Loop through the items and subtract their weight from the random number
    // until we find the selected item
    foreach (EnemySpawnerItemSO item in _weightedList)
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
