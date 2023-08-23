using System.Collections.Generic;
using UnityEngine;
using FSM;
using LlamAcademy.Sensors;
using UnityEngine.AI;
using Lix.Core;

[RequireComponent(typeof(Animator), typeof(NavMeshAgent))]
public class Zombie : Enemy
{
    [Header("Sensors")]
    [SerializeField]
    private HumanSensor _followPlayerSensor;

    private StateMachine<EnemyState, EnemyStateEvent> _enemyFSM;

    public override void Awake()
    {
        base.Awake();

        _enemyFSM = new();

        // Add States
        _enemyFSM.AddState(EnemyState.Idle, new ZombieIdleState(false, this));
        _enemyFSM.AddState(EnemyState.Chase, new ZombieChaseState(true, this));
        _enemyFSM.AddState(EnemyState.Attack, new ZombieAttackState(true, this, OnAttack));

        // Add Transitions
        _enemyFSM.AddTriggerTransition(EnemyStateEvent.DetectTarget, new Transition<EnemyState>(EnemyState.Idle, EnemyState.Chase));
        _enemyFSM.AddTriggerTransition(EnemyStateEvent.LostTarget, new Transition<EnemyState>(EnemyState.Chase, EnemyState.Idle));

        _enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Idle, EnemyState.Chase,
            (transition) => TargetsInRange.Count > 0)
        );

        _enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Chase, EnemyState.Idle,
            (transition) => TargetsInRange.Count == 0)
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
        _followPlayerSensor.OnHumanEnter += FollowPlayerSensor_OnPlayerEnter;
        _followPlayerSensor.OnHumanExit += FollowPlayerSensor_OnPlayerExit;
        // _rangeAttackPlayerSensor.OnPlayerEnter += RangeAttackPlayerSensor_OnPlayerEnter;
        // _rangeAttackPlayerSensor.OnPlayerExit += RangeAttackPlayerSensor_OnPlayerExit;
    }

    private void OnDisable()
    {
        _followPlayerSensor.OnHumanEnter -= FollowPlayerSensor_OnPlayerEnter;
        _followPlayerSensor.OnHumanExit -= FollowPlayerSensor_OnPlayerExit;
        // _rangeAttackPlayerSensor.OnPlayerEnter -= RangeAttackPlayerSensor_OnPlayerEnter;
        // _rangeAttackPlayerSensor.OnPlayerExit -= RangeAttackPlayerSensor_OnPlayerExit;
    }
    private void FixedUpdate()
    {
        _enemyFSM.OnLogic();
    }

    private void FollowPlayerSensor_OnPlayerExit(GameObject player)
    {
        TargetsInRange.Remove(player);
        _enemyFSM.Trigger(EnemyStateEvent.LostTarget);
    }

    private void FollowPlayerSensor_OnPlayerEnter(GameObject player)
    {
        TargetsInRange.Add(player);
        _enemyFSM.Trigger(EnemyStateEvent.DetectTarget);
    }

    private void OnAttack(State<EnemyState, EnemyStateEvent> State)
    {
        GameObject closest = GetClosestTarget();

        if (!ShouldMelee(null))
        {
            return;
        }

        transform.LookAt(closest.transform.position);
        LastAttackTime = Time.time;

        Human human = closest.GetComponent<Human>();
        human.TakeDamage(20);
    }
}
