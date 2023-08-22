using System.Collections.Generic;
using UnityEngine;
using FSM;
using LlamAcademy.Sensors;
using UnityEngine.AI;
using Lix.Core;

[RequireComponent(typeof(Animator), typeof(NavMeshAgent))]
public class Human : Enemy
{
    [Header("Attack Config")]
    [SerializeField]
    [Range(0.1f, 5f)]
    private float _attackCooldown = 2;

    [Header("Sensors")]
    [SerializeField]
    private ZombieSensor _followPlayerSensor;
    [SerializeField]
    private ZombieSensor _rangeAttackPlayerSensor;
    [SerializeField]
    private ZombieSensor _meleePlayerSensor;

    [Space]
    [Header("Debug Info")]
    [SerializeField]
    private bool _isInMeleeRange;
    [SerializeField]
    private bool _isInChasingRange;
    [SerializeField]
    private float _lastAttackTime;

    private StateMachine<EnemyState, EnemyStateEvent> _enemyFSM;

    public override void Awake()
    {
        base.Awake();

        _enemyFSM = new();

        // Add States
        _enemyFSM.AddState(EnemyState.Idle, new HumanIdleState(false, this));
        _enemyFSM.AddState(EnemyState.Chase, new HumanChaseState(true, this));
        _enemyFSM.AddState(EnemyState.Attack, new HumanAttackState(true, this, OnAttack));

        // Add Transitions
        _enemyFSM.AddTriggerTransition(EnemyStateEvent.DetectTarget, new Transition<EnemyState>(EnemyState.Idle, EnemyState.Chase));
        _enemyFSM.AddTriggerTransition(EnemyStateEvent.LostTarget, new Transition<EnemyState>(EnemyState.Chase, EnemyState.Idle));

        _enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Idle, EnemyState.Chase,
            (transition) => _isInChasingRange && TargetsInRange.Count > 0)
        );

        _enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Chase, EnemyState.Idle,
            (transition) => !_isInChasingRange || TargetsInRange.Count == 0)
        );

        // Attack Transitions
        _enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Chase, EnemyState.Attack, ShouldMelee, true));
        _enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Idle, EnemyState.Attack, ShouldMelee, true));
        _enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Attack, EnemyState.Chase, IsNotWithinIdleRange));
        _enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Attack, EnemyState.Idle, IsWithinIdleRange));

        _enemyFSM.Init();
    }

    private void OnEnable()
    {
        _followPlayerSensor.OnZombieEnter += FollowPlayerSensor_OnPlayerEnter;
        _followPlayerSensor.OnZombieExit += FollowPlayerSensor_OnPlayerExit;
        // _rangeAttackPlayerSensor.OnPlayerEnter += RangeAttackPlayerSensor_OnPlayerEnter;
        // _rangeAttackPlayerSensor.OnPlayerExit += RangeAttackPlayerSensor_OnPlayerExit;
        _meleePlayerSensor.OnZombieEnter += MeleePlayerSensor_OnPlayerEnter;
        _meleePlayerSensor.OnZombieExit += MeleePlayerSensor_OnPlayerExit;
    }

    private void OnDisable()
    {
        _followPlayerSensor.OnZombieEnter -= FollowPlayerSensor_OnPlayerEnter;
        _followPlayerSensor.OnZombieExit -= FollowPlayerSensor_OnPlayerExit;
        // _rangeAttackPlayerSensor.OnPlayerEnter -= RangeAttackPlayerSensor_OnPlayerEnter;
        // _rangeAttackPlayerSensor.OnPlayerExit -= RangeAttackPlayerSensor_OnPlayerExit;
        _meleePlayerSensor.OnZombieEnter -= MeleePlayerSensor_OnPlayerEnter;
        _meleePlayerSensor.OnZombieExit -= MeleePlayerSensor_OnPlayerExit;
    }

    private void FollowPlayerSensor_OnPlayerExit(GameObject player)
    {
        TargetsInRange.Remove(player);
        _enemyFSM.Trigger(EnemyStateEvent.LostTarget);
        _isInChasingRange = TargetsInRange.Count > 0;
    }

    private void FollowPlayerSensor_OnPlayerEnter(GameObject player)
    {
        TargetsInRange.Add(player);
        _enemyFSM.Trigger(EnemyStateEvent.DetectTarget);
        _isInChasingRange = true;
    }

    private bool ShouldMelee(Transition<EnemyState> Transition) =>
        _lastAttackTime + _attackCooldown <= Time.time
               && _isInMeleeRange;

    private void MeleePlayerSensor_OnPlayerExit(GameObject player) => _isInMeleeRange = false;

    private void MeleePlayerSensor_OnPlayerEnter(GameObject player) => _isInMeleeRange = true;

    private void OnAttack(State<EnemyState, EnemyStateEvent> State)
    {
        transform.LookAt(GetClosestTarget().transform.position);
        _lastAttackTime = Time.time;
    }

    private void Update()
    {
        _enemyFSM.OnLogic();
    }
}
