using System.Collections.Generic;
using UnityEngine;
using Lix.Core;

public class LevelSL : ServiceLocatorRegisterMono
{
  [SerializeField] private Player _player;
  [SerializeField] private ZombiePoolManager _zombiePoolManager;

  [SerializeField] private ZombieTypeSelector _zombieTypeSelector;

  public override void RegisterServices()
  {
    ServiceLocator.Register(new ServiceDescriptor(_player), false);
    ServiceLocator.Register(new ServiceDescriptor(_zombiePoolManager), false);
    ServiceLocator.Register(new ServiceDescriptor(_zombieTypeSelector), false);
  }

  public override void UnregisterServices()
  {
    ServiceLocator.Remove(_player);
    ServiceLocator.Remove(_zombiePoolManager);
    ServiceLocator.Remove(_zombieTypeSelector);
  }
}