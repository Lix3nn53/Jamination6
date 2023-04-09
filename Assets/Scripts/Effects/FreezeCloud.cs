using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeCloud : AreaOfEffect
{
  [SerializeField] private float _slowFactor = 0.2f;
  [SerializeField] private float _slowDuration = 1f;
  public override void OnAreaOfEffect(CombatUnit combatUnit)
  {
    PhysicsBasedCharacterController physicsBasedCharacterController = combatUnit.GetComponent<PhysicsBasedCharacterController>();

    if (physicsBasedCharacterController != null)
    {
      physicsBasedCharacterController.Slow(_slowFactor, _slowDuration);
    }
    else
    {
      PhysicsBasedEnemy physicsBasedEnemy = combatUnit.GetComponent<PhysicsBasedEnemy>();
      if (physicsBasedEnemy != null)
      {
        physicsBasedEnemy.Slow(_slowFactor, _slowDuration);
      }
    }
  }
}
