using FSM;
using System;
using UnityEngine;

public class ZombieAttackState : EnemyStateBase
{
    public ZombieAttackState(
        bool needsExitTime,
        Zombie Enemy,
        Action<State<EnemyState, EnemyStateEvent>> onEnter,
        float ExitTime = 0.33f) : base(needsExitTime, Enemy, ExitTime, onEnter) { }

    public override void OnEnter()
    {
        Agent.isStopped = true;
        base.OnEnter();
        // Animator.Play("Attack");

        // var propertyBlock = new MaterialPropertyBlock();
        // propertyBlock.SetColor("_Color", Color.red);
        // Enemy.MeshRenderer.SetPropertyBlock(propertyBlock);
    }
}
