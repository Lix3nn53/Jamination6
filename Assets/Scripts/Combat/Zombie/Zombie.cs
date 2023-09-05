using System.Collections.Generic;
using UnityEngine;
using FSM;
using LlamAcademy.Sensors;
using UnityEngine.AI;
using Lix.Core;

[RequireComponent(typeof(Animator), typeof(NavMeshAgent))]
public class Zombie : EnemyWithAI
{
    [HideInInspector] public ZombiePool ZombiePool; // WindPool is set in WindPool.OnTakeFromPool

    [Header("Sensors")]
    [SerializeField]
    private HumanSensor _humanSensor;

    public StateMachine<EnemyState, EnemyStateEvent> EnemyFSM;

    public override void Awake()
    {
        base.Awake();

        EnemyFSM = new();

        // Add States
        EnemyFSM.AddState(EnemyState.Idle, new ZombieIdleState(false, this));
        EnemyFSM.AddState(EnemyState.Chase, new ZombieChaseState(true, this));
        EnemyFSM.AddState(EnemyState.Attack, new ZombieAttackState(true, this, OnAttack));

        // Add Transitions
        EnemyFSM.AddTriggerTransition(EnemyStateEvent.DetectTarget, new Transition<EnemyState>(EnemyState.Idle, EnemyState.Chase));
        EnemyFSM.AddTriggerTransition(EnemyStateEvent.LostTarget, new Transition<EnemyState>(EnemyState.Chase, EnemyState.Idle));

        EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Idle, EnemyState.Chase,
            (transition) => TargetsInRange.Count > 0)
        );

        EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Chase, EnemyState.Idle,
            (transition) => TargetsInRange.Count == 0)
        );

        // Attack Transitions
        EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Chase, EnemyState.Attack, ShouldMelee, true));
        EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Idle, EnemyState.Attack, ShouldMelee, true));
        EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Attack, EnemyState.Chase, IsNotWithinIdleRange));
        EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Attack, EnemyState.Idle, IsWithinIdleRange));

        EnemyFSM.Init();
    }

    public override void OnEnable()
    {
        base.OnEnable();

        _humanSensor.OnEnter += HumanSensor_OnHumanEnter;
        _humanSensor.OnExit += HumanSensor_OnHumanExit;
        // _rangeAttackPlayerSensor.OnPlayerEnter += RangeAttackPlayerSensor_OnPlayerEnter;
        // _rangeAttackPlayerSensor.OnPlayerExit += RangeAttackPlayerSensor_OnPlayerExit;
    }

    public virtual void OnDisable()
    {
        _humanSensor.OnEnter -= HumanSensor_OnHumanEnter;
        _humanSensor.OnExit -= HumanSensor_OnHumanExit;
        // _rangeAttackPlayerSensor.OnPlayerEnter -= RangeAttackPlayerSensor_OnPlayerEnter;
        // _rangeAttackPlayerSensor.OnPlayerExit -= RangeAttackPlayerSensor_OnPlayerExit;
    }
    private void FixedUpdate()
    {
        EnemyFSM.OnLogic();
    }

    private void HumanSensor_OnHumanEnter(GameObject human)
    {
        TargetsInRange.Add(human);
        EnemyFSM.Trigger(EnemyStateEvent.DetectTarget);
    }

    private void HumanSensor_OnHumanExit(GameObject human)
    {
        TargetsInRange.Remove(human);
        EnemyFSM.Trigger(EnemyStateEvent.LostTarget);
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

        Enemy enemy = closest.GetComponent<Enemy>();
        enemy.TakeDamage(Damage);

        OnAttackPerformed();
    }

    public virtual void OnAttackPerformed()
    {
    }

    public override void Die()
    {
        if (ZombiePool != null)
        {
            ZombiePool.Pool.Release(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
