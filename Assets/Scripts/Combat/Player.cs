using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lix.Core;

public class Player : CombatUnit
{
  private GameManager _gameManager;
  public override void Start()
  {
    base.Start();
    _gameManager = ServiceLocator.Get<GameManager>();
  }
  public override void OnDeath()
  {
    Debug.Log("Player died");
    _gameManager.OnGameOverEvent?.Invoke(_gameManager.Score);
  }

  public override void OnMelee()
  {
  }

  public override void OnTakeDamage()
  {
    _gameManager.OnPlayerHealthChangeEvent?.Invoke(this.Health);
  }
}
