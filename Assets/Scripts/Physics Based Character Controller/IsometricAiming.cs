using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class IsometricAiming : MonoBehaviour
{
  #region Datamembers

  #region Editor Settings

  [SerializeField] private LayerMask groundMask;

  #endregion
  #region Private Fields

  private Camera mainCamera;

  #endregion

  #endregion


  #region Methods

  #region Unity Callbacks

  private void Start()
  {
    // Cache the camera, Camera.main is an expensive operation.
    mainCamera = Camera.main;
  }

  #endregion

  public (bool success, Vector3 position) GetMousePosition()
  {
    // if (mainCamera == null)
    // {
    //   Debug.LogError("No camera found in scene.");
    //   mainCamera = Camera.main;
    //   // The camera is not cached, return with failure.
    //   return (success: false, position: Vector3.zero);
    // }

    var ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

    if (Physics.Raycast(ray, out var hitInfo, 10000f, groundMask))
    {
      // The Raycast hit something, return with the position.
      return (success: true, position: hitInfo.point);
    }
    else
    {
      // The Raycast did not hit anything.
      return (success: false, position: Vector3.zero);
    }
  }

  #endregion
}
