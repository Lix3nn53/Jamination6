using System.Collections.Generic;
using UnityEngine;
using FSM;
using LlamAcademy.Sensors;
using UnityEngine.AI;
using Lix.Core;
using Crystal;

[RequireComponent(typeof(Animator), typeof(NavMeshAgent))]
public class Human : EnemyWithAI
{
    [Header("Sensors")]
    [SerializeField]
    private ZombieSensor _zombieSensor;

    private StateMachine<EnemyState, EnemyStateEvent> _enemyFSM;

    private ZombiePoolManager _zombiePool;

    private ZombieTypeSelector _zombieTypeSelector;

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

    private void Start()
    {
        _zombiePool = ServiceLocator.Get<ZombiePoolManager>();
        _zombieTypeSelector = ServiceLocator.Get<ZombieTypeSelector>();
    }

    public override void OnEnable()
    {
        base.OnEnable();

        _zombieSensor.OnZombieEnter += FollowPlayerSensor_OnZombieEnter;
        _zombieSensor.OnZombieExit += FollowPlayerSensor_OnZombieExit;
        // _rangeAttackPlayerSensor.OnPlayerEnter += RangeAttackPlayerSensor_OnPlayerEnter;
        // _rangeAttackPlayerSensor.OnPlayerExit += RangeAttackPlayerSensor_OnPlayerExit;
    }

    private void OnDisable()
    {
        _zombieSensor.OnZombieEnter -= FollowPlayerSensor_OnZombieEnter;
        _zombieSensor.OnZombieExit -= FollowPlayerSensor_OnZombieExit;
        // _rangeAttackPlayerSensor.OnPlayerEnter -= RangeAttackPlayerSensor_OnPlayerEnter;
        // _rangeAttackPlayerSensor.OnPlayerExit -= RangeAttackPlayerSensor_OnPlayerExit;
    }
    private void FixedUpdate()
    {
        _enemyFSM.OnLogic();
    }

    private void FollowPlayerSensor_OnZombieExit(GameObject player)
    {
        TargetsInRange.Remove(player);
        _enemyFSM.Trigger(EnemyStateEvent.LostTarget);
    }

    private void FollowPlayerSensor_OnZombieEnter(GameObject player)
    {
        TargetsInRange.Add(player);
        _enemyFSM.Trigger(EnemyStateEvent.DetectTarget);
    }

    private void OnAttack(State<EnemyState, EnemyStateEvent> State)
    {
        GameObject closest = DetermineTarget();

        if (!ShouldMelee(null))
        {
            return;
        }

        transform.LookAt(closest.transform.position);
        LastAttackTime = Time.time;

        Zombie zombie = closest.GetComponent<Zombie>();
        if (zombie != null)
        {
            zombie.TakeDamage(Damage);
        }
        else
        {
            Player player = closest.GetComponent<Player>();
            player.TakeDamage(Damage);
        }
    }

    public override void Die()
    {
        base.Die();

        // Random enum ZombieType
        ZombieType zombieType = _zombieTypeSelector.activeType;

        // instantiate zombie
        Zombie zombie = _zombiePool.GetZombie(zombieType);
        zombie.transform.position = transform.position;
        zombie.transform.rotation = transform.rotation;
        zombie.gameObject.SetActive(true);
    }
}
