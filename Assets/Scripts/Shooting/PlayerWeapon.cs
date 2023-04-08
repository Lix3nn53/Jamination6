using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Weapon", menuName = "Gamejam/Player Weapon")]
public class PlayerWeapon : ScriptableObject
{
  [SerializeField] private GameObject _weaponPrefab;

  public GameObject Equip(Transform weaponParent)
  {
    GameObject weapon = Instantiate(_weaponPrefab);
    weapon.transform.SetParent(weaponParent);
    weapon.transform.localPosition = Vector3.zero;
    weapon.transform.localRotation = Quaternion.identity;

    return weapon;
  }
}
