using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using LlamAcademy.Sensors;
using UnityEngine.AI;
using Lix.Core;

[RequireComponent(typeof(Animator), typeof(NavMeshAgent))]
public abstract class EnemyWithAI : Enemy
{
    [Header("References")]
    public List<GameObject> TargetsInRange = new List<GameObject>(); // Collection of players in range
    public float ClosestDistance = Mathf.Infinity;

    private Animator _animator;
    public NavMeshAgent Agent;

    [Header("Config")]
    public float MeleeAttackCooldown = 2f;
    public float MeleeRange = 4f;
    public int Damage = 10;

    public float LastAttackTime;

    public virtual void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();

        _animator = GetComponent<Animator>();
    }

    public bool IsWithinIdleRange(Transition<EnemyState> Transition) =>
        Agent.remainingDistance <= Agent.stoppingDistance;

    public bool IsNotWithinIdleRange(Transition<EnemyState> Transition) =>
        !IsWithinIdleRange(Transition);

    public bool ShouldMelee(Transition<EnemyState> Transition)
    {
        DetermineTarget();

        return LastAttackTime + MeleeAttackCooldown <= Time.time && ClosestDistance <= MeleeRange;
    }

    public virtual GameObject DetermineTarget()
    {
        GameObject closestTarget = null;
        ClosestDistance = Mathf.Infinity;

        List<GameObject> validTargets = new List<GameObject>();

        foreach (var target in TargetsInRange)
        {
            if (target == null) continue;

            var distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance < ClosestDistance)
            {
                ClosestDistance = distance;
                closestTarget = target;
            }

            validTargets.Add(target);
        }

        TargetsInRange = validTargets;

        return closestTarget;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, MeleeRange);
    }
}
