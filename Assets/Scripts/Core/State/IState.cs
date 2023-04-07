using System.Collections;
using System.Collections.Generic;

namespace Lix.Core
{
  public interface IState
  {
    public void Enter();
    public void Execute();
    public void Exit();
  }
}