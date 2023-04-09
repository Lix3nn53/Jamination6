using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lix.Core;

public class Enemy : CombatUnit
{
  [SerializeField] private int _scoreValue = 10;
  [SerializeField] private Color _colorOnDeath = Color.grey;
  private PhysicsBasedEnemy _physicsBasedEnemy;
  private GameManager _gameManager;
  public override void Start()
  {
    base.Start();
    _physicsBasedEnemy = GetComponent<PhysicsBasedEnemy>();
    _gameManager = ServiceLocator.Get<GameManager>();
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

    _gameManager.AddScore(_scoreValue);
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
