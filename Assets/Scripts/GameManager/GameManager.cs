using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Lix.Core;

public class GameManager : MonoBehaviour
{
  [SerializeField] private int score;
  public int Score => score;

  [SerializeField] private int scorePerSecond;

  // Score to complete level
  [SerializeField] private int currentLevel = 1;
  [SerializeField] private int targetScoreBase;
  [SerializeField] private int targetScoreAddition;

  // Events
  public delegate void OnScoreChange(int score);
  public OnScoreChange OnScoreChangeEvent;
  public delegate void OnPlayerHealthChange(int currentHP);
  public OnPlayerHealthChange OnPlayerHealthChangeEvent;
  public delegate void OnZombieTypeChange(ZombieType activeType);
  public OnZombieTypeChange OnZombieTypeChangeEvent;

  public delegate void OnGameOver(int score);
  public OnGameOver OnGameOverEvent;

  public delegate void OnLevelComplete(int score);
  public OnLevelComplete OnLevelCompleteEvent;

  public void AddScore(int add)
  {
    this.score += add;
    OnScoreChangeEvent?.Invoke(score);
  }

  public void GameOver()
  {
    int targetScoreTotal = GetTotalTargetScore();

    int targetScore = GetTargetScore(currentLevel);
    int prevTargetScoreTotal = targetScoreTotal - GetTargetScore(currentLevel);

    OnGameOverEvent?.Invoke(score);
    this.currentLevel = 1;
  }

  public int GetTargetScore(int level)
  {
    if (level <= 0)
    {
      return 0;
    }

    return ((level - 1) * targetScoreAddition) + targetScoreBase;
  }

  public int GetTotalTargetScore()
  {
    int targetScoreTotal = 0;

    for (int i = 1; i <= currentLevel; i++)
    {
      targetScoreTotal += GetTargetScore(i);
    }

    return targetScoreTotal;
  }
}
