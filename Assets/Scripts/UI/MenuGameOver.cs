using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lix.Core;
using TMPro;

public class MenuGameOver : MonoBehaviour
{

  [SerializeField] private TMP_Text scoreText;
  [SerializeField] private Button _buttonMainMenu;

  private SceneLoader sceneLoader;
  private GameManager gameManager;


  private void Start()
  {
    sceneLoader = ServiceLocator.Get<SceneLoader>();
    this.gameManager = ServiceLocator.Get<GameManager>();
    this.gameManager.OnGameOverEvent += OnGameOver;

    foreach (Transform child in transform)
    {
      child.gameObject.SetActive(false);
    }
  }

  private void OnEnable()
  {
    _buttonMainMenu.onClick.AddListener(OnButtonMainMenuClick);
  }

  private void OnDisable()
  {
    _buttonMainMenu.onClick.RemoveListener(OnButtonMainMenuClick);
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

  public void OnButtonMainMenuClick()
  {
    Debug.Log("OnButtonMainMenuClick");
    sceneLoader.Load(0);
  }
}
