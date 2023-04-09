using System.Collections.Generic;
using UnityEngine;
using Lix.Core;

public class LevelSL : ServiceLocatorRegisterMono
{
  [SerializeField] private Player _player;

  public override void RegisterServices()
  {
    ServiceLocator.Register(new ServiceDescriptor(_player), false);
  }

  public override void UnregisterServices()
  {
    ServiceLocator.Remove(_player);
  }
}