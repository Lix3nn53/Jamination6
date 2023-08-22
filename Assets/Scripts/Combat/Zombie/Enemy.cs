using System.Collections.Generic;
using UnityEngine;
using FSM;
using LlamAcademy.Sensors;
using UnityEngine.AI;
using Lix.Core;

namespace LlamAcademy.FSM
{
    [RequireComponent(typeof(Animator), typeof(NavMeshAgent))]
    public abstract class Enemy : MonoBehaviour
    {
        [Header("References")]
        public List<GameObject> TargetsInRange = new List<GameObject>(); // Collection of players in range
        public MeshRenderer MeshRenderer;

        public Animator Animator;
        public NavMeshAgent Agent;

        public virtual void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            Animator = GetComponent<Animator>();
        }

        public bool IsWithinIdleRange(Transition<EnemyState> Transition) =>
            Agent.remainingDistance <= Agent.stoppingDistance;

        public bool IsNotWithinIdleRange(Transition<EnemyState> Transition) =>
            !IsWithinIdleRange(Transition);

        public GameObject GetClosestTarget()
        {
            GameObject closestTarget = null;
            float closestDistance = Mathf.Infinity;

            foreach (var target in TargetsInRange)
            {
                var distance = Vector3.Distance(transform.position, target.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = target;
                }
            }

            return closestTarget;
        }
    }
}
