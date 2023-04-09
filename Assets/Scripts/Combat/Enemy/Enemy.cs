using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CombatUnit
{
  private PhysicsBasedEnemy _physicsBasedEnemy;
  public override void Start()
  {
    base.Start();
    _physicsBasedEnemy = GetComponent<PhysicsBasedEnemy>();
  }

  public override void OnDeath()
  {
    _physicsBasedEnemy.enabled = false;
    Destroy(gameObject, 0.5f);
  }

  public override void OnMelee()
  {

  }

  public override void OnTakeDamage()
  {
    Debug.Log("Enemy took damage");
  }
}
