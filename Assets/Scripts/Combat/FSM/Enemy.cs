using UnityEngine;
using FSM;
using LlamAcademy.Sensors;
using UnityEngine.AI;
using Lix.Core;

namespace LlamAcademy.FSM
{
    [RequireComponent(typeof(Animator), typeof(NavMeshAgent))]
    public class Enemy : MonoBehaviour
    {
        [Header("References")]
        private Player _player;
        [SerializeField]
        private Spit _spitPrefab;
        [SerializeField]
        private ParticleSystem _bounceImpactParticleSystem;

        [Header("Attack Config")]
        [SerializeField]
        [Range(0.1f, 5f)]
        private float _attackCooldown = 2;
        [SerializeField]
        [Range(1, 20f)]
        private float _rollCooldown = 17;
        [SerializeField]
        [Range(1, 10f)]
        private float _bounceCooldown = 10;

        [Header("Sensors")]
        [SerializeField]
        private PlayerSensor _followPlayerSensor;
        [SerializeField]
        private PlayerSensor _rangeAttackPlayerSensor;
        [SerializeField]
        private PlayerSensor _meleePlayerSensor;
        [SerializeField]
        private ImpactSensor _rollImpactSensor;

        [Space]
        [Header("Debug Info")]
        [SerializeField]
        private bool _isInMeleeRange;
        [SerializeField]
        private bool _isInSpitRange;
        [SerializeField]
        private bool _isInChasingRange;
        [SerializeField]
        private float _lastAttackTime;
        [SerializeField]
        private float _lastBounceTime;
        [SerializeField]
        private float _lastRollTime;

        private StateMachine<EnemyState, StateEvent> _enemyFSM;
        private Animator _animator;
        private NavMeshAgent _agent;

        private void Awake()
        {
            _player = ServiceLocator.Get<Player>();
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _enemyFSM = new();

            // Add States
            _enemyFSM.AddState(EnemyState.Idle, new IdleState(false, this));
            _enemyFSM.AddState(EnemyState.Chase, new ChaseState(true, this, _player.transform));
            _enemyFSM.AddState(EnemyState.Spit, new SpitState(true, this, _spitPrefab, OnAttack));
            _enemyFSM.AddState(EnemyState.Bounce, new BounceState(true, this, _bounceImpactParticleSystem, OnBounce));
            _enemyFSM.AddState(EnemyState.Roll, new RollState(true, this, OnRoll));
            _enemyFSM.AddState(EnemyState.Attack, new AttackState(true, this, OnAttack));

            // Add Transitions
            _enemyFSM.AddTriggerTransition(StateEvent.DetectPlayer, new Transition<EnemyState>(EnemyState.Idle, EnemyState.Chase));
            _enemyFSM.AddTriggerTransition(StateEvent.LostPlayer, new Transition<EnemyState>(EnemyState.Chase, EnemyState.Idle));
            _enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Idle, EnemyState.Chase,
                (transition) => _isInChasingRange
                                && Vector3.Distance(_player.transform.position, transform.position) > _agent.stoppingDistance)
            );
            _enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Chase, EnemyState.Idle,
                (transition) => !_isInChasingRange
                                || Vector3.Distance(_player.transform.position, transform.position) <= _agent.stoppingDistance)
            );

            // Roll Transitions
            _enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Chase, EnemyState.Roll, ShouldRoll, true));
            _enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Idle, EnemyState.Roll, ShouldRoll, true));
            _enemyFSM.AddTriggerTransition(StateEvent.RollImpact,
                new Transition<EnemyState>(EnemyState.Roll, EnemyState.Bounce, null, true));
            _enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Roll, EnemyState.Chase, IsNotWithinIdleRange));
            _enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Roll, EnemyState.Idle, IsWithinIdleRange));

            // Bounce transitions
            _enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Chase, EnemyState.Bounce, ShouldBounce));
            _enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Idle, EnemyState.Bounce, ShouldBounce));
            _enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Bounce, EnemyState.Chase, IsNotWithinIdleRange));
            _enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Bounce, EnemyState.Idle, IsWithinIdleRange));

            // Spit Transitions
            _enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Chase, EnemyState.Spit, ShouldSpit, true));
            _enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Idle, EnemyState.Spit, ShouldSpit, true));
            _enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Spit, EnemyState.Chase, IsNotWithinIdleRange));
            _enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Spit, EnemyState.Idle, IsWithinIdleRange));

            // Attack Transitions
            _enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Chase, EnemyState.Attack, ShouldMelee, true));
            _enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Idle, EnemyState.Attack, ShouldMelee, true));
            _enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Attack, EnemyState.Chase, IsNotWithinIdleRange));
            _enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Attack, EnemyState.Idle, IsWithinIdleRange));

            _enemyFSM.Init();
        }

        private void OnEnable()
        {
            _followPlayerSensor.OnPlayerEnter += FollowPlayerSensor_OnPlayerEnter;
            _followPlayerSensor.OnPlayerExit += FollowPlayerSensor_OnPlayerExit;
            _rangeAttackPlayerSensor.OnPlayerEnter += RangeAttackPlayerSensor_OnPlayerEnter;
            _rangeAttackPlayerSensor.OnPlayerExit += RangeAttackPlayerSensor_OnPlayerExit;
            _meleePlayerSensor.OnPlayerEnter += MeleePlayerSensor_OnPlayerEnter;
            _meleePlayerSensor.OnPlayerExit += MeleePlayerSensor_OnPlayerExit;
            _rollImpactSensor.OnCollision += RollImpactSensor_OnCollision;
        }

        private void OnDisable()
        {
            _followPlayerSensor.OnPlayerEnter -= FollowPlayerSensor_OnPlayerEnter;
            _followPlayerSensor.OnPlayerExit -= FollowPlayerSensor_OnPlayerExit;
            _rangeAttackPlayerSensor.OnPlayerEnter -= RangeAttackPlayerSensor_OnPlayerEnter;
            _rangeAttackPlayerSensor.OnPlayerExit -= RangeAttackPlayerSensor_OnPlayerExit;
            _meleePlayerSensor.OnPlayerEnter -= MeleePlayerSensor_OnPlayerEnter;
            _meleePlayerSensor.OnPlayerExit -= MeleePlayerSensor_OnPlayerExit;
            _rollImpactSensor.OnCollision -= RollImpactSensor_OnCollision;
        }

        private void RollImpactSensor_OnCollision(Collision Collision)
        {
            _enemyFSM.Trigger(StateEvent.RollImpact);
            _lastRollTime = Time.time;
            _lastAttackTime = Time.time;
            _rollImpactSensor.gameObject.SetActive(false);
        }

        private void FollowPlayerSensor_OnPlayerExit(Vector3 LastKnownPosition)
        {
            _enemyFSM.Trigger(StateEvent.LostPlayer);
            _isInChasingRange = false;
        }

        private void FollowPlayerSensor_OnPlayerEnter(Transform Player)
        {
            _enemyFSM.Trigger(StateEvent.DetectPlayer);
            _isInChasingRange = true;
        }

        private bool ShouldRoll(Transition<EnemyState> Transition) =>
            _lastRollTime + _rollCooldown <= Time.time
                   && _isInChasingRange;

        private bool ShouldBounce(Transition<EnemyState> Transition) =>
            _lastBounceTime + _bounceCooldown <= Time.time
                   && _isInMeleeRange;

        private bool ShouldSpit(Transition<EnemyState> Transition) =>
            _lastAttackTime + _attackCooldown <= Time.time
                   && !_isInMeleeRange
                   && _isInSpitRange;

        private bool ShouldMelee(Transition<EnemyState> Transition) =>
            _lastAttackTime + _attackCooldown <= Time.time
                   && _isInMeleeRange;

        private bool IsWithinIdleRange(Transition<EnemyState> Transition) =>
            _agent.remainingDistance <= _agent.stoppingDistance;

        private bool IsNotWithinIdleRange(Transition<EnemyState> Transition) =>
            !IsWithinIdleRange(Transition);

        private void MeleePlayerSensor_OnPlayerExit(Vector3 LastKnownPosition) => _isInMeleeRange = false;

        private void MeleePlayerSensor_OnPlayerEnter(Transform Player) => _isInMeleeRange = true;

        private void RangeAttackPlayerSensor_OnPlayerExit(Vector3 LastKnownPosition) => _isInSpitRange = false;

        private void RangeAttackPlayerSensor_OnPlayerEnter(Transform Player) => _isInSpitRange = true;

        private void OnAttack(State<EnemyState, StateEvent> State)
        {
            transform.LookAt(_player.transform.position);
            _lastAttackTime = Time.time;
        }

        private void OnBounce(State<EnemyState, StateEvent> State)
        {
            transform.LookAt(_player.transform.position);
            _lastAttackTime = Time.time;
            _lastBounceTime = Time.time;
        }

        private void OnRoll(State<EnemyState, StateEvent> State)
        {
            _rollImpactSensor.gameObject.SetActive(true);
            transform.LookAt(_player.transform.position);
            _lastRollTime = Time.time;
        }

        private void Update()
        {
            _enemyFSM.OnLogic();
        }
    }
}
