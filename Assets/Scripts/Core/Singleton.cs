using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Lix.Core
{
  // I don't use this class, but it's a good example of how to use the singleton pattern.
  public abstract class Singleton<T> : MonoBehaviour where T : Component
  {
    public bool DontDestroy = false;

    #region Fields

    /// <summary>
    /// The instance.
    /// </summary>
    private T instance;

    #endregion

    #region Properties

    /// <summary>
    /// Gets the instance.
    /// </summary>
    /// <value>The instance.</value>
    public T Instance
    {
      get
      {
        if (instance == null)
        {
          InternalDebug.LogWarning("GET SINGLETON BEFORE AWAKE, Instance of " + typeof(T));
        }

        return instance;
      }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Use this for initialization.
    /// </summary>
    protected virtual void Awake()
    {
      if (instance == null)
      {
        instance = this as T;
        if (DontDestroy)
        {
          DontDestroyOnLoad(gameObject);
        }
      }
      else
      {
        Destroy(gameObject);
      }
    }

    #endregion

  }
}