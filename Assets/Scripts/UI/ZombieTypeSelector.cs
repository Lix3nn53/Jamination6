using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lix.Core;
using System;
using Unity.VisualScripting;

public class ZombieTypeSelector : MonoBehaviour
{
  [SerializeField] private ToggleGroup _zombieTypes;
  [SerializeField] private Toggle defaultActiveToggle;

  private GameManager gameManager;
  private ZombieTypeToggleSprites zombieTypeToggleSprite;
  private Slider virusProgressBar;
  private Player _player;
  public ZombieType activeType;

  private void Start()
  {
    this.gameManager = ServiceLocator.Get<GameManager>();
    _player = ServiceLocator.Get<Player>();
    this.gameManager.OnZombieTypeChangeEvent += OnZombieTypeChange;
    _zombieTypes = GetComponentInChildren<ToggleGroup>();
    virusProgressBar = GetComponentInChildren<Slider>();

    defaultActiveToggle.isOn = true;
  }

  private void OnEnable()
  {
    foreach (Toggle toggle in _zombieTypes.GetComponentsInChildren<Toggle>())
    {
      Debug.Log(toggle);
      toggle.onValueChanged.AddListener((bool value) => onToggleValueChanged(toggle));
    }
  }

  private void OnDisable()
  {
    foreach (Toggle toggle in _zombieTypes.GetComponentsInChildren<Toggle>())
    {
      toggle.onValueChanged.RemoveListener((bool value) => onToggleValueChanged(toggle));
    }
  }

  public void OnZombieTypeChange(ZombieType activeType)
  {
    Debug.Log("active type is " + activeType);

  }

  private void OnDestroy()
  {
    this.gameManager.OnZombieTypeChangeEvent -= OnZombieTypeChange;
  }
  public void onToggleValueChanged(Toggle toggle)
  {
    zombieTypeToggleSprite = toggle.GetComponent<ZombieTypeToggleSprites>();
    zombieTypeToggleSprite.ChangeSprite(toggle);

    if (toggle.isOn)
    {
      Enum.TryParse(toggle.name, true, out ZombieType type);
      activeType = type;
      Debug.Log("ChangeActiveType = " + activeType);

      switch (activeType)
      {
        case ZombieType.Attacker:
          virusProgressBar.value = _player.attackerVirus;
          break;
        case ZombieType.Healer:
          virusProgressBar.value = _player.healerVirus;
          break;
        case ZombieType.Tank:
          virusProgressBar.value = _player.tankVirus;
          break;
        case ZombieType.Collector:
          virusProgressBar.value = _player.collectorVirus;
          break;
        default:
          break;
      }
    }
  }
}
