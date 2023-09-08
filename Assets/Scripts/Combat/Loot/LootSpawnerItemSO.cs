using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LootSpawnerItem", menuName = "Gamejam/LootSpawnerItem", order = 1)]
public class LootSpawnerItemSO : ScriptableObject
{
  [SerializeField] public GameObject Prefab;
  [SerializeField] public LootType lootType;
  [SerializeField] public int Weight;
}
