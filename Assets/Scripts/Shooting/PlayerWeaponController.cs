using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
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

  // Cooldown and reload
  private bool _isReloading = false;
  [SerializeField] private Slider _reloadSlider;

  // Start is called before the first frame update
  void Awake()
  {
    EquipWeapon(_currentWeaponIndex);
    _inputListener = ServiceLocator.Get<InputListener>();

    _reloadSlider.value = 1f;
  }

  private void OnEnable()
  {
    _inputListener.GetAction(InputActionType.Fire).performed += Fire;
  }

  private void OnDisable()
  {
    _inputListener.GetAction(InputActionType.Fire).performed -= Fire;
  }

  public void Fire(UnityEngine.InputSystem.InputAction.CallbackContext context)
  {
    if (_isReloading)
    {
      return;
    }

    _currentLauncher.LaunchToMouse();

    StartCoroutine(StartReload());
    StartCoroutine(AnimateReloadSlider());
  }

  private IEnumerator StartReload()
  {
    _isReloading = true;
    yield return new WaitForSeconds(_currentLauncher.ReloadTime);
    _isReloading = false;
  }

  private IEnumerator AnimateReloadSlider()
  {
    float time = 0f;
    while (time < _currentLauncher.ReloadTime)
    {
      time += Time.deltaTime;
      _reloadSlider.value = time / _currentLauncher.ReloadTime;
      yield return null;
    }
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
