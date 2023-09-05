using FSM;
using System;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyStateBase : State<EnemyState, EnemyStateEvent>
{
    protected readonly EnemyWithAI Enemy;
    protected readonly NavMeshAgent Agent;
    protected readonly Animator Animator;
    protected bool RequestedExit;
    protected float ExitTime;

    protected readonly Action<State<EnemyState, EnemyStateEvent>> onEnter;
    protected readonly Action<State<EnemyState, EnemyStateEvent>> onLogic;
    protected readonly Action<State<EnemyState, EnemyStateEvent>> onExit;
    protected readonly Func<State<EnemyState, EnemyStateEvent>, bool> canExit;

    public EnemyStateBase(bool needsExitTime,
        EnemyWithAI Enemy,
        float ExitTime = 0.1f,
        Action<State<EnemyState, EnemyStateEvent>> onEnter = null,
        Action<State<EnemyState, EnemyStateEvent>> onLogic = null,
        Action<State<EnemyState, EnemyStateEvent>> onExit = null,
        Func<State<EnemyState, EnemyStateEvent>, bool> canExit = null)
    {
        this.Enemy = Enemy;
        this.onEnter = onEnter;
        this.onLogic = onLogic;
        this.onExit = onExit;
        this.canExit = canExit;
        this.ExitTime = ExitTime;
        this.needsExitTime = needsExitTime;
        Agent = Enemy.GetComponent<NavMeshAgent>();
        Animator = Enemy.GetComponent<Animator>();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        RequestedExit = false;
        onEnter?.Invoke(this);
    }

    public override void OnLogic()
    {
        base.OnLogic();
        if (RequestedExit && timer.Elapsed >= ExitTime)
        {
            fsm.StateCanExit();
        }
    }

    public override void OnExitRequest()
    {
        if (!needsExitTime || canExit != null && canExit(this))
        {
            fsm.StateCanExit();
        }
        else
        {
            RequestedExit = true;
        }
    }
}
