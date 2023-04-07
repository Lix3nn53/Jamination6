using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lix.Core;
using TMPro;

public class MenuUpgrade : MonoBehaviour
{
  // Outer Dependencies

  GameManager gameManager;

  // Inner Dependencies
  [SerializeField] private TMP_Text balanceText;
  [SerializeField] private TMP_Text costStationText;
  [SerializeField] private TMP_Text costMissileText;
  [SerializeField] private TMP_Text costReloadText;
  [SerializeField] private TMP_Text levelStationText;
  [SerializeField] private TMP_Text levelMissileText;
  [SerializeField] private TMP_Text levelReloadText;

  void Start()
  {
    gameManager = ServiceLocator.Get<GameManager>();

    balanceText.text = "Balance: " + gameManager.Money + " $";
  }

  public void ButtonUpgradeStation()
  {

  }

  public void ButtonUpgradeMissile()
  {

  }

  public void ButtonUpgradeReload()
  {

  }
}
