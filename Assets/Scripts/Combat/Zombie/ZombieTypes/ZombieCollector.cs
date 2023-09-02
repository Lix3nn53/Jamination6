using System.Collections.Generic;
using UnityEngine;
using FSM;
using LlamAcademy.Sensors;
using UnityEngine.AI;
using Lix.Core;

public class ZombieCollector : Zombie
{
    public override void OnAttackPerformed()
    {
        Debug.Log("Collector Attack");
    }
}
