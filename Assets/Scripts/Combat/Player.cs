using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lix.Core;
using LlamAcademy.Sensors;

public class Player : CombatUnit
{
  [SerializeField] private HumanSensor _humanSensor;
  private GameManager _gameManager;
  private Player _player;

  public int damagerVirus;
  public int healerVirus;
  public int tankVirus;
  public int collectorVirus;
  public int scoreCollectable;
  public int damageBuff;
  public int defenseBuff;
  public int movementBuff;

  public float MeleeRange = 4f;
  public float MeleeAttackCooldown = 2f;

  private List<Human> _humansInRange = new List<Human>();
  private float _closestDistance = Mathf.Infinity;
  private float _lastAttackTime;

  public override void Start()
  {
    base.Start();
    _gameManager = ServiceLocator.Get<GameManager>();
    _player = ServiceLocator.Get<Player>();
  }
  public void OnEnable()
  {
    _humanSensor.OnHumanEnter += HumanSensor_OnHumanEnter;
  }
  public void OnDisable()
  {
    _humanSensor.OnHumanEnter -= HumanSensor_OnHumanEnter;
  }
  public override void OnDeath()
  {
    _gameManager.OnGameOverEvent?.Invoke(_gameManager.Score);
    _player.gameObject.SetActive(false);
  }

  public override void OnMelee()
  {
  }

  public override void OnTakeDamage()
  {
    _gameManager.OnPlayerHealthChangeEvent?.Invoke(this.Health);
  }

  private void HumanSensor_OnHumanEnter(GameObject human)
  {
    _humansInRange.Add(human.GetComponent<Human>());
  }

  private void FixedUpdate()
  {
    MeleeAttack();
  }

  public bool ShouldMelee()
  {
    GetClosestTarget();

    return _lastAttackTime + MeleeAttackCooldown <= Time.time && _closestDistance <= MeleeRange;
  }

  private void MeleeAttack()
  {
    Human closest = GetClosestTarget();

    if (!ShouldMelee())
    {
      return;
    }

    transform.LookAt(closest.transform.position);
    _lastAttackTime = Time.time;

    Human human = closest.GetComponent<Human>();
    if (human != null)
    {
      human.TakeDamage(MeleeDamage);
    }
  }

  public Human GetClosestTarget()
  {
    Human closestTarget = null;
    _closestDistance = Mathf.Infinity;

    foreach (var target in _humansInRange)
    {
      if (target == null) continue;

      var distance = Vector3.Distance(transform.position, target.transform.position);
      if (distance < _closestDistance)
      {
        _closestDistance = distance;
        closestTarget = target;
      }
    }

    return closestTarget;
  }
}
