using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Lix.Core;

public class GameManager : MonoBehaviour
{
  public int startYear = 2022;
  [SerializeField] private int score;
  [SerializeField] private int money;
  public int Money { get { return money; } private set { money = value; } }
  [SerializeField] private int moneyPerHit = 100000;
  [SerializeField] private int yearPeriod = 10;
  [SerializeField] private int targetScoreBase = 60;
  [SerializeField] private int targetScoreAddition = 20;

  // State
  public int currentLevel = 1;

  // Events

  public delegate void OnScoreChange(int score);
  public event OnScoreChange OnScoreChangeEvent;

  public delegate void OnGameOver(int year);
  public event OnGameOver OnGameOverEvent;

  public delegate void OnLevelComplete(int year);
  public event OnLevelComplete OnLevelCompleteEvent;

  public delegate void OnMissileHitStar();
  public event OnMissileHitStar OnMissileHitStarEvent;

  public delegate void OnMoneyChange(int money);
  public event OnMoneyChange OnMoneyChangeEvent;

  void Start()
  {
    this.startYear = DateTime.Now.Year;

    StartScore();
    OnMoneyChangeEvent?.Invoke(money);
    SceneManager.sceneLoaded += OnSceneLoaded;
  }

  void OnSceneLoaded(Scene scene, LoadSceneMode mode)
  {
    InternalDebug.Log("OnSceneLoaded: " + scene.name);
    if (scene.buildIndex == 1) // Only scene where score is updated
    {
      StartScore();
    }
  }

  public void StartScore()
  {
    InternalDebug.Log("StartLevel");
    this.score = 0;

    InvokeRepeating("AddScore", 0f, yearPeriod);
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
      OnLevelCompleteEvent?.Invoke(this.startYear + this.score);
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

    int currentYear = this.startYear + prevTargetScoreTotal + score;
    OnGameOverEvent?.Invoke(currentYear);
    this.currentLevel = 1;
    this.money = 0;
  }

  public void MissileHitStar()
  {
    money += moneyPerHit;
    OnMoneyChangeEvent?.Invoke(money);
    OnMissileHitStarEvent?.Invoke();
  }

  public int GetTargetScore(int level)
  {
    if (level <= 0)
    {
      return 0;
    }

    return ((level - 1) * targetScoreAddition) + targetScoreBase;
  }

  public int AddMoney(int amount)
  {
    money += amount;
    OnMoneyChangeEvent?.Invoke(money);
    return money;
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
