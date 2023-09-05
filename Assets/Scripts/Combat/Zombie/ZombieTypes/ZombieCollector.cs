using System.Collections.Generic;
using UnityEngine;
using FSM;
using LlamAcademy.Sensors;
using UnityEngine.AI;
using Lix.Core;

public class ZombieCollector : Zombie
{
    [SerializeField]
    private LootSensor _lootSensor;
    public override void OnEnable()
    {
        base.OnEnable();

        _lootSensor.OnEnter += OnLootEnter;
        _lootSensor.OnExit += OnLootExit;
    }

    public override void OnDisable()
    {
        _lootSensor.OnEnter -= OnLootEnter;
        _lootSensor.OnExit -= OnLootExit;
    }

    private void OnLootEnter(GameObject human)
    {
        TargetsInRange.Add(human);
        EnemyFSM.Trigger(EnemyStateEvent.DetectTarget);
    }

    private void OnLootExit(GameObject human)
    {
        TargetsInRange.Remove(human);
        EnemyFSM.Trigger(EnemyStateEvent.LostTarget);
    }
    public override void OnAttackPerformed()
    {
        Debug.Log("Collector Attack");
    }

    public override GameObject DetermineTarget()
    {
        GameObject closestTarget = null;
        ClosestDistance = Mathf.Infinity;
        bool closestTargetIsLoot = false;

        List<GameObject> validTargets = new List<GameObject>();
        bool currentTargetIsLoot = false;

        foreach (var target in TargetsInRange)
        {
            if (target == null) continue;

            Enemy enemy = target.GetComponent<Enemy>();
            if (enemy == null)
            {
                Loot loot = target.GetComponent<Loot>();
                if (loot != null)
                {
                    currentTargetIsLoot = true;
                }
            }
            else
            {
                currentTargetIsLoot = false;
            }

            var distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance < ClosestDistance)
            {
                if (currentTargetIsLoot) // New target is closer and is wall, so set as result target
                {
                    ClosestDistance = distance;
                    closestTarget = target;
                    closestTargetIsLoot = true;
                }
                else
                {
                    if (currentTargetIsLoot)
                    {
                        continue; // New target is closer but result target was wall, so ignore
                    }
                    ClosestDistance = distance;
                    closestTarget = target;
                    closestTargetIsLoot = false;
                }
            }
            else if (!closestTargetIsLoot) // New target is not closer but result target was human, so check if new target is wall
            {
                if (currentTargetIsLoot) // New target is wall while result target is human, so set as result target
                {
                    ClosestDistance = distance;
                    closestTarget = target;
                    closestTargetIsLoot = true;
                }
            }

            validTargets.Add(target);
        }

        TargetsInRange = validTargets;

        return closestTarget;
    }
}
