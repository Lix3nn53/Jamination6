using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpawnerItem", menuName = "Gamejam/EnemySpawnerItem", order = 1)]
public class EnemySpawnerItemSO : ScriptableObject
{
  public GameObject Prefab;
  public int Weight;
}
