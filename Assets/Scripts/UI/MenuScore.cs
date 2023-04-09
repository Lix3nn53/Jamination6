using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lix.Core;
using TMPro;

public class MenuScore : MonoBehaviour
{
  [SerializeField] private TMP_Text text;

  private GameManager gameManager;

  // Start is called before the first frame update
  void Start()
  {
    this.gameManager = ServiceLocator.Get<GameManager>();
    this.gameManager.OnScoreChangeEvent += OnScoreChange;

    OnScoreChange(this.gameManager.Score);
  }

  public void OnScoreChange(int score)
  {
    text.text = score + "";
  }

  private void OnDestroy()
  {
    this.gameManager.OnScoreChangeEvent -= OnScoreChange;
  }
}
