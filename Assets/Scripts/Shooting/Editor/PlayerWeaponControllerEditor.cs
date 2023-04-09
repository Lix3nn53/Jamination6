using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerWeaponController))]
public class PlayerWeaponControllerEditor : Editor
{
  public override void OnInspectorGUI()
  {
    base.OnInspectorGUI();

    PlayerWeaponController controller = (PlayerWeaponController)target;

    if (GUILayout.Button("Next Weapon"))
    {
      controller.NextWeapon();
    }
  }
}