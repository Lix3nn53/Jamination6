using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lix.Core;

public class LootFactory : FactoryMono<Loot>
{
  [SerializeField] Loot _attackerVirusPrefab;
  [SerializeField] Loot _healerVirusPrefab;
  [SerializeField] Loot _tankVirusPrefab;
  [SerializeField] Loot _collectorVirusPrefab;
  [SerializeField] Loot _scoreCollectablePrefab;
  [SerializeField] Loot _damageBuffPrefab;
  [SerializeField] Loot _defenseBuffPrefab;
  [SerializeField] Loot _movementBuffPrefab;

  public Loot Get(LootType type, Vector3 position, Quaternion rotation)
  {
    switch (type)
    {
      case LootType.AttackerVirus:
        return Instantiate(_attackerVirusPrefab, position, rotation);
      case LootType.HealerVirus:
        return Instantiate(_healerVirusPrefab, position, rotation);
      case LootType.TankVirus:
        return Instantiate(_tankVirusPrefab, position, rotation);
       case LootType.CollectorVirus:
        return Instantiate(_collectorVirusPrefab, position, rotation);
      case LootType.ScoreCollectable:
        return Instantiate(_scoreCollectablePrefab, position, rotation);
      case LootType.DamageBuff:
        return Instantiate(_damageBuffPrefab, position, rotation);
      case LootType.DefenseBuff:
        return Instantiate(_defenseBuffPrefab, position, rotation);
       case LootType.MovementBuff:
        return Instantiate(_movementBuffPrefab, position, rotation);
      default:
       return Instantiate(_attackerVirusPrefab, position, rotation);
    }
  }

  public override Loot Create()
  {
    return Get(LootType.AttackerVirus, Vector3.zero, Quaternion.identity);
  }

  public Loot Create(LootType type)
  {
    return Get(type, Vector3.zero, Quaternion.identity);
  }
}
