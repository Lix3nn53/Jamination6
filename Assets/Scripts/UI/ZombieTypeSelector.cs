using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lix.Core;
using System;

public class ZombieTypeSelector : MonoBehaviour
{
  [SerializeField] private ToggleGroup _zombieTypes;

  [SerializeField] private ZombieType activeType;

  private GameManager gameManager;
  private ZombieTypeToggleSprites zombieTypeToggleSprite;

  private void Start()
  {
    this.gameManager = ServiceLocator.Get<GameManager>();
    this.gameManager.OnZombieTypeChangeEvent += OnZombieTypeChange;
    _zombieTypes = GetComponentInChildren<ToggleGroup>();
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
    
    if(toggle.isOn){
      Enum.TryParse(toggle.name, true, out ZombieType type);
      activeType = type;
      Debug.Log(toggle.name +"ChangeActiveType = " + activeType);
    }
  }

  public ZombieType GetActiveType()
  {
    return activeType;
  }

}
