using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lix.Core;
using TMPro;

public class MenuGameOver : MonoBehaviour
{

  [SerializeField] private TMP_Text score;

  private GameManager gameManager;

  private void Start()
  {
    this.gameManager = ServiceLocator.Get<GameManager>();
    this.gameManager.OnGameOverEvent += OnGameOver;

    foreach (Transform child in transform)
    {
      child.gameObject.SetActive(false);
    }
  }
  public void OnGameOver(int year)
  {
    score.text = "You have reached year " + year;

    foreach (Transform child in transform)
    {
      child.gameObject.SetActive(true);
    }
  }

  private void OnDestroy()
  {
    this.gameManager.OnGameOverEvent -= OnGameOver;
  }
}
