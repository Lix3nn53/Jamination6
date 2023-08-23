using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using LlamAcademy.Sensors;
using UnityEngine.AI;
using Lix.Core;

[RequireComponent(typeof(Animator), typeof(NavMeshAgent))]
public abstract class Enemy : MonoBehaviour
{
    [Header("References")]
    public List<GameObject> TargetsInRange = new List<GameObject>(); // Collection of players in range
    public float ClosestDistance = Mathf.Infinity;
    public MeshRenderer MeshRenderer;

    public Animator Animator;
    public NavMeshAgent Agent;

    [Header("Config")]
    public float MeleeAttackCooldown = 2f;
    public float MeleeRange = 4f;
    public int MaxHealth = 100;
    private int Health = 100;
    public int Damage = 10;

    [Space]
    [Header("Debug Info")]
    [SerializeField]
    public float LastAttackTime;
    private bool _isTakingDamage = false;
    private float _damageTakeInterval = 0.2f;

    public virtual void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        bool a = Agent.Warp(transform.position);
        Debug.Log("Warp: " + a);

        Animator = GetComponent<Animator>();

        Health = MaxHealth;
    }

    public bool IsWithinIdleRange(Transition<EnemyState> Transition) =>
        Agent.remainingDistance <= Agent.stoppingDistance;

    public bool IsNotWithinIdleRange(Transition<EnemyState> Transition) =>
        !IsWithinIdleRange(Transition);

    public bool ShouldMelee(Transition<EnemyState> Transition)
    {
        GetClosestTarget();

        return LastAttackTime + MeleeAttackCooldown <= Time.time && ClosestDistance <= MeleeRange;
    }

    public GameObject GetClosestTarget()
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

    public void TakeDamage(int damage)
    {
        if (_isTakingDamage) return;

        Health -= damage;
        if (Health <= 0)
        {
            Die();
        }
        else
        {
            DamageTintColor();
            StartCoroutine(DamageTakeCooldown());
        }
    }

    private void DamageTintColor()
    {
        foreach (Material material in MeshRenderer.materials)
        {
            StartCoroutine(DamageTintColorForOne(material));
        }
    }

    private IEnumerator DamageTintColorForOne(Material material)
    {
        Color originalColor = material.color;
        material.color = Color.red;
        yield return new WaitForSeconds(_damageTakeInterval);
        material.color = originalColor;
    }

    private IEnumerator DamageTakeCooldown()
    {
        _isTakingDamage = true;
        yield return new WaitForSeconds(_damageTakeInterval);
        _isTakingDamage = false;
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, MeleeRange);
    }
}
