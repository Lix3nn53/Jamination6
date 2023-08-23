using FSM;
using System;
using UnityEngine;

public class BounceState : EnemyStateBase
{
    private ParticleSystem BounceParticleSystem;

    public BounceState(
        bool needsExitTime,
        Zombie Enemy,
        ParticleSystem BounceParticleSystem,
        Action<State<EnemyState, EnemyStateEvent>> onEnter,
        float ExitTime = 0.33f) : base(needsExitTime, Enemy, ExitTime, onEnter)
    {
        this.BounceParticleSystem = BounceParticleSystem;
    }

    public override void OnEnter()
    {
        Agent.isStopped = true;
        base.OnEnter();
        Animator.Play("Bounce");
        BounceParticleSystem.Play();

        // var propertyBlock = new MaterialPropertyBlock();
        // propertyBlock.SetColor("_Color", Color.blue);
        // Enemy.MeshRenderer.SetPropertyBlock(propertyBlock);
    }
}

