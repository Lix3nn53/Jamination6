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

  public delegate void OnGameOver(int score);
  public OnGameOver OnGameOverEvent;

  public delegate void OnLevelComplete(int score);
  public OnLevelComplete OnLevelCompleteEvent;

  void Start()
  {
    StartScore();
    SceneManager.sceneLoaded += OnSceneLoaded;
  }

  private void OnDestroy()
  {
    SceneManager.sceneLoaded -= OnSceneLoaded;
  }

  void OnSceneLoaded(Scene scene, LoadSceneMode mode)
  {
    InternalDebug.Log("OnSceneLoaded: " + scene.name);
    if (scene.buildIndex == 1) // Only scene where score is updated
    {
      StartScore();
    }
    else
    {
      StopScore();
    }
  }

  public void StartScore()
  {
    InternalDebug.Log("StartLevel");
    this.score = 0;

    InvokeRepeating("AddScore", 1f, scorePerSecond);
  }

  public void StopScore()
  {
    CancelInvoke("AddScore");
  }

  void AddScore()
  {
    this.score++;
    OnScoreChangeEvent?.Invoke(score);

    int targetScore = GetTargetScore(currentLevel);

    if (score >= targetScore)
    {
      Time.timeScale = 0;
      OnLevelCompleteEvent?.Invoke(this.score);
      StopScore();
      this.currentLevel++;
    }
  }

  public void GameOver()
  {
    StopScore();
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
