using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lix.Core
{
  public abstract class StateMachine : MonoBehaviour
  {
    public IState CurrentState { get; private set; }

    public void ChangeState(IState newState)
    {
      InternalDebug.Log("Changing state");
      if (CurrentState != null)
      {
        InternalDebug.Log("from " + CurrentState.GetType().Name);
        CurrentState.Exit();
      }

      CurrentState = newState;

      if (CurrentState != null)
      {
        InternalDebug.Log(" to " + newState.GetType().Name);
        CurrentState.Enter();
      }
    }

    protected virtual void Update()
    {
      if (CurrentState != null)
      {
        CurrentState.Execute();
      }
    }
  }
}