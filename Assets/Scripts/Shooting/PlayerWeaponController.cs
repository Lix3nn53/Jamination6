using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Lix.Core;

public class PlayerWeaponController : MonoBehaviour
{
  [SerializeField] private GameObject _cursor;
  [SerializeField] private LayerMask _cursorMask;
  [SerializeField] private Transform _weaponParent;
  [SerializeField] private PlayerWeapon[] _weaponInventory;

  private InputListener _inputListener;
  private int _currentWeaponIndex = 0;
  private LaunchProjectile _currentLauncher;

  // Start is called before the first frame update
  void Start()
  {
    EquipWeapon(_currentWeaponIndex);

    _inputListener = ServiceLocator.Get<InputListener>();
    _inputListener.GetAction(InputActionType.Fire).performed += Fire;
  }

  public void Fire(UnityEngine.InputSystem.InputAction.CallbackContext context)
  {
    _currentLauncher.LaunchToMouse();

  }

  private void Update()
  {
    ShowMouseTarget();
  }

  public void ShowMouseTarget()
  {
    Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
    RaycastHit hit;

    if (Physics.Raycast(ray, out hit, 1000f, _cursorMask))
    {
      _cursor.transform.position = hit.point + Vector3.up * 0.1f;
      _cursor.SetActive(true);
    }
    else
    {
      _cursor.SetActive(false);
    }
  }

  public void NextWeapon()
  {
    _currentWeaponIndex++;
    if (_currentWeaponIndex >= _weaponInventory.Length)
    {
      _currentWeaponIndex = 0;
    }

    EquipWeapon(_currentWeaponIndex);
  }

  private void EquipWeapon(int index)
  {
    foreach (Transform child in _weaponParent)
    {
      Destroy(child.gameObject);
    }

    GameObject weapon = _weaponInventory[index].Equip(_weaponParent);
    _currentLauncher = weapon.GetComponent<LaunchProjectile>();
  }
}
