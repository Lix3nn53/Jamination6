using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lix.Core;
using TMPro;

public class MenuGameOver : MonoBehaviour
{

  [SerializeField] private TMP_Text scoreText;

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
  public void OnGameOver(int scoreValue)
  {
    scoreText.text = "You score is " + scoreValue;

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
