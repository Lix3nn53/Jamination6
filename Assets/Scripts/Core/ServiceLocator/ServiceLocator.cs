using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Lix.Core
{
  public class ServiceLocator : MonoBehaviour
  {
    private static IDictionary<Type, ServiceDescriptor> serviceDescriptors = new Dictionary<Type, ServiceDescriptor>();

    private static IDictionary<Type, ServiceDescriptor> GetServiceDescriptors()
    {
      return serviceDescriptors;
    }

    public static void Register(ServiceDescriptor serviceDescriptor, bool dontDestroyOnLoad = false)
    {
      IDictionary<Type, ServiceDescriptor> serviceDescriptors = GetServiceDescriptors();

      // if (serviceDescriptor.Implementation is MonoBehaviour)
      // {
      //   Debug.Log(string.Format("New service {0} is registered!", serviceDescriptor.ServiceType), (MonoBehaviour)serviceDescriptor.Implementation);
      // }
      // else
      // {
      //   Debug.Log(string.Format("New service {0} is registered!", serviceDescriptor.ServiceType));
      // }

      serviceDescriptors.Remove(serviceDescriptor.ServiceType);
      serviceDescriptors.Add(serviceDescriptor.ServiceType, serviceDescriptor);

      if (dontDestroyOnLoad)
      {
        DontDestroyOnLoad((MonoBehaviour)serviceDescriptor.Implementation);
      }
    }

    public static void Remove(object implementation)
    {
      Type serviceType = implementation.GetType();
      if (serviceDescriptors.ContainsKey(serviceType))
      {
        serviceDescriptors.Remove(serviceType);
        // Debug.Log(string.Format("Service {0} is removed!", serviceType));
      }
    }

    private static object Get(Type serviceType, bool silent)
    {
      IDictionary<Type, ServiceDescriptor> serviceDescriptors = GetServiceDescriptors();

      if (!serviceDescriptors.ContainsKey(serviceType))
      {
        if (!silent)
        {
          throw new Exception($"Service of type {serviceType.Name} is not registered");
        }
        return null;
      }

      var serviceDescriptor = serviceDescriptors[serviceType];

      // Return implementation if it is already created
      if (serviceDescriptor.Implementation != null)
      {
        return serviceDescriptor.Implementation;
      }

      // Create implementation if it is not created yet
      Type actualType = serviceDescriptor.ImplementationType ?? serviceDescriptor.ServiceType;
      if (actualType.IsAbstract || actualType.IsInterface)
      {
        throw new Exception($"Can't create instance of abstract or interface type {actualType.Name}");
      }

      System.Reflection.ConstructorInfo constructorInfo = actualType.GetConstructors()[0];

      object[] parameters = constructorInfo.GetParameters().Select(parameter => Get(parameter.ParameterType, silent)).ToArray();

      var implementation = Activator.CreateInstance(actualType, parameters);

      serviceDescriptor.Implementation = implementation;

      return implementation;
    }

    public static T Get<T>(bool silent = false)
    {
      return (T)Get(typeof(T), silent);
    }
  }
}