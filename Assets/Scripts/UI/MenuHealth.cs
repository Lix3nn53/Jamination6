using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Lix.Core;

public class MenuHealth : MonoBehaviour
{
  [SerializeField] private Slider slider;
  [SerializeField] private TMP_Text text;

  // Outer Dependencies
  private GameManager _gameManager;

  private Player _player;

  // Start is called before the first frame update
  void Start()
  {
    _gameManager = ServiceLocator.Get<GameManager>();
    _player = ServiceLocator.Get<Player>();

    _gameManager.OnPlayerHealthChangeEvent += OnHealthChange;

    OnHealthChange(_player.Health);
  }

  private void OnHealthChange(int currentHP)
  {
    int maxHP = _player.MaxHealth;

    this.text.text = "Health " + currentHP + " / " + maxHP;

    slider.value = (float)(currentHP) / (float)(maxHP);
  }

  private void OnDestroy()
  {
    _gameManager.OnPlayerHealthChangeEvent -= OnHealthChange;
  }
}
