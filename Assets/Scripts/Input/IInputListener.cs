using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum InputActionType { Move, Pause, Fire, Aim, Jump }
public interface IInputListener
{
  InputAction GetAction(InputActionType type);
}