using UnityEngine;


namespace Lix.Core
{
  public abstract class ServiceLocatorRegisterMono : MonoBehaviour, ServiceLocatorRegister
  {
    private void Awake()
    {
      RegisterServices();
    }

    private void OnDestroy()
    {
      UnregisterServices();
    }

    public abstract void RegisterServices();

    public abstract void UnregisterServices();
  }
}