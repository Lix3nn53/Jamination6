using System;

namespace Lix.Core
{
  public class ServiceDescriptor
  {
    public Type ServiceType { get; set; }
    public Type ImplementationType { get; set; }
    public object Implementation { get; set; }
    public ServiceDescriptor(object implementation)
    {
      this.ServiceType = implementation.GetType();
      this.Implementation = implementation;
    }
    public ServiceDescriptor(object implementation, Type serviceType)
    {
      this.ServiceType = serviceType;
      this.Implementation = implementation;
    }
    public ServiceDescriptor(Type serviceType)
    {
      this.ServiceType = serviceType;
    }
    public ServiceDescriptor(Type serviceType, Type implementationType)
    {
      this.ServiceType = serviceType;
      this.ImplementationType = implementationType;
    }
  }
}