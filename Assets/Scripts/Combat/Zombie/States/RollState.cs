using FSM;
using System;
using UnityEngine;

public class RollState : EnemyStateBase
{
    public RollState(
        bool needsExitTime,
        Zombie Enemy,
        Action<State<EnemyState, EnemyStateEvent>> onEnter,
        float ExitTime = 3f) : base(needsExitTime, Enemy, ExitTime, onEnter) { }

    public override void OnEnter()
    {
        Agent.isStopped = true;
        base.OnEnter();
        Animator.Play("Roll");

        var propertyBlock = new MaterialPropertyBlock();
        propertyBlock.SetColor("_Color", Color.green);
        Enemy.MeshRenderer.SetPropertyBlock(propertyBlock);
    }

    public override void OnLogic()
    {
        Agent.Move(1.5f * Agent.speed * Time.deltaTime * Agent.transform.forward);
        base.OnLogic();
    }
}
