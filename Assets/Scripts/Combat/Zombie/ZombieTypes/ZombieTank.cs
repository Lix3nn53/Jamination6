using System.Collections.Generic;
using UnityEngine;
using FSM;
using LlamAcademy.Sensors;
using UnityEngine.AI;
using Lix.Core;

public class ZombieTank : Zombie
{
    [SerializeField]
    private HumanWallSensor _humanWallSensor;
    public override void OnEnable()
    {
        base.OnEnable();

        _humanWallSensor.OnEnter += OnWallEnter;
        _humanWallSensor.OnExit += OnWallExit;
    }

    public override void OnDisable()
    {
        _humanWallSensor.OnEnter -= OnWallEnter;
        _humanWallSensor.OnExit -= OnWallExit;
    }

    private void OnWallEnter(GameObject human)
    {
        TargetsInRange.Add(human);
        EnemyFSM.Trigger(EnemyStateEvent.DetectTarget);
    }

    private void OnWallExit(GameObject human)
    {
        TargetsInRange.Remove(human);
        EnemyFSM.Trigger(EnemyStateEvent.LostTarget);
    }

    public override void OnAttackPerformed()
    {
        Debug.Log("Tank Attack");
    }

    public override GameObject DetermineTarget()
    {
        GameObject closestTarget = null;
        ClosestDistance = Mathf.Infinity;
        HumanType closestTargetType = HumanType.Human;

        List<GameObject> validTargets = new List<GameObject>();

        foreach (var target in TargetsInRange)
        {
            if (target == null) continue;

            Enemy enemy = target.GetComponent<Enemy>();

            var distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance < ClosestDistance)
            {
                if (enemy is HumanWall) // New target is closer and is wall, so set as result target
                {
                    ClosestDistance = distance;
                    closestTarget = target;
                    closestTargetType = HumanType.Wall;
                }
                else if (enemy is Human)
                {
                    if (closestTargetType == HumanType.Wall)
                    {
                        continue; // New target is closer but result target was wall, so ignore
                    }
                    ClosestDistance = distance;
                    closestTarget = target;
                    closestTargetType = HumanType.Human;
                }
            }
            else if (closestTargetType == HumanType.Human) // New target is not closer but result target was human, so check if new target is wall
            {
                if (enemy is HumanWall) // New target is wall while result target is human, so set as result target
                {
                    ClosestDistance = distance;
                    closestTarget = target;
                    closestTargetType = HumanType.Wall;
                }
            }

            validTargets.Add(target);
        }

        TargetsInRange = validTargets;

        return closestTarget;
    }
}
