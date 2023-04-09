using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CombatUnit
{
  [SerializeField] private Color _colorOnDeath = Color.grey;
  private PhysicsBasedEnemy _physicsBasedEnemy;
  public override void Start()
  {
    base.Start();
    _physicsBasedEnemy = GetComponent<PhysicsBasedEnemy>();
  }

  public override void OnDeath()
  {
    _physicsBasedEnemy.enabled = false;
    // Change Layer
    gameObject.layer = LayerMask.NameToLayer("Default");

    foreach (Material material in Materials)
    {
      material.color = _colorOnDeath;
    }

    // Remove CombatUnit
    Destroy(this);
  }

  public override void OnMelee()
  {
  }

  public override void OnTakeDamage()
  {
    Debug.Log("Enemy took damage");
  }
}
