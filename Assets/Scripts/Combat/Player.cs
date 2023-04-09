using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lix.Core;

public class Player : CombatUnit
{
  private GameManager _gameManager;
  private Player _player;
  public override void Start()
  {
    base.Start();
    _gameManager = ServiceLocator.Get<GameManager>();
    _player = ServiceLocator.Get<Player>();
  }
  public override void OnDeath()
  {
    _gameManager.OnGameOverEvent?.Invoke(_gameManager.Score);
    _player.gameObject.SetActive(false);
  }

  public override void OnMelee()
  {
  }

  public override void OnTakeDamage()
  {
    _gameManager.OnPlayerHealthChangeEvent?.Invoke(this.Health);
  }
}
