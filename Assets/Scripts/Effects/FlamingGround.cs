using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamingGround : AreaOfEffect
{
  [SerializeField] private int _damage = 5;
  public override void OnAreaOfEffect(CombatUnit combatUnit)
  {
    combatUnit.TakeDamage(_damage);
  }
}
