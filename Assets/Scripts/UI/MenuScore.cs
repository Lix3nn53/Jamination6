using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Lix.Core;

public class MenuScore : MonoBehaviour
{
  [SerializeField] private Slider slider;
  [SerializeField] private TMP_Text text;

  // Outer Dependencies
  private GameManager gameManager;

  // Start is called before the first frame update
  void Start()
  {
    this.gameManager = ServiceLocator.Get<GameManager>();

    this.gameManager.OnScoreChangeEvent += OnScoreChange;
  }

  private void OnScoreChange(int score)
  {
    int targetScoreTotal = gameManager.GetTotalTargetScore();

    int targetScore = gameManager.GetTargetScore(gameManager.currentLevel);
    int prevTargetScoreTotal = targetScoreTotal - gameManager.GetTargetScore(gameManager.currentLevel);

    int currentYear = this.gameManager.startYear + prevTargetScoreTotal + score;
    int targetYear = this.gameManager.startYear + targetScoreTotal;

    this.text.text = "Year " + currentYear + " / " + targetYear;

    slider.value = (float)(score) / (float)(targetScore);
  }

  private void OnDestroy()
  {
    this.gameManager.OnScoreChangeEvent -= OnScoreChange;
  }
}
