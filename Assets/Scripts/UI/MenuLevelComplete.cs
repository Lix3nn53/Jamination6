using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lix.Core;
using TMPro;

public class MenuLevelComplete : MonoBehaviour
{
  [SerializeField] private TMP_Text score;

  private GameManager gameManager;

  private void Start()
  {
    this.gameManager = ServiceLocator.Get<GameManager>();
    this.gameManager.OnLevelCompleteEvent += OnLevelComplete;

    foreach (Transform child in transform)
    {
      child.gameObject.SetActive(false);
    }
  }
  public void OnLevelComplete(int year)
  {
    score.text = "You are at year " + year;

    foreach (Transform child in transform)
    {
      child.gameObject.SetActive(true);
    }
  }

  private void OnDestroy()
  {
    this.gameManager.OnLevelCompleteEvent -= OnLevelComplete;
  }
}
