using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lix.Core
{
  public abstract class FactoryMono<T> : MonoBehaviour, IFactory<T>
  {
    public abstract T Create();
  }
}