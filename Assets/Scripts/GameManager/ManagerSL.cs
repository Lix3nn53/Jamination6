using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Lix.Core;

public class ManagerSL : ServiceLocatorRegisterMono
{
  [SerializeField] private EventSystem eventSystem;
  [SerializeField] private SceneLoader sceneLoader;
  [SerializeField] private InputListener inputListener;
  [SerializeField] private AudioManager audioManager;
  [SerializeField] private GameManager gameManager;

  public override void RegisterServices()
  {
    ServiceLocator.Register(new ServiceDescriptor(eventSystem), true);
    ServiceLocator.Register(new ServiceDescriptor(sceneLoader), true);
    ServiceLocator.Register(new ServiceDescriptor(inputListener), true);
    ServiceLocator.Register(new ServiceDescriptor(audioManager), true);
    ServiceLocator.Register(new ServiceDescriptor(gameManager), true);
  }

  public override void UnregisterServices()
  {
    // ServiceLocator.Remove(inputListener);
    // ServiceLocator.Remove(audioManager);
    // ServiceLocator.Remove(gameManager);
  }
}