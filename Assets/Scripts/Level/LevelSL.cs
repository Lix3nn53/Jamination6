using System.Collections.Generic;
using UnityEngine;
using Lix.Core;

public class LevelSL : ServiceLocatorRegisterMono
{
  [SerializeField] private Player _player;
  [SerializeField] private ZombiePool _zombiePool;

  public override void RegisterServices()
  {
    ServiceLocator.Register(new ServiceDescriptor(_player), false);
    ServiceLocator.Register(new ServiceDescriptor(_zombiePool), false);
  }

  public override void UnregisterServices()
  {
    ServiceLocator.Remove(_player);
    ServiceLocator.Remove(_zombiePool);
  }
}