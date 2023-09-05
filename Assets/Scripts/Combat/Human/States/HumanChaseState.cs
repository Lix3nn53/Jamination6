using UnityEngine;

public class HumanChaseState : EnemyStateBase
{
    private Transform Target;

    public HumanChaseState(bool needsExitTime, Human Human) : base(needsExitTime, Human)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();

        GameObject target = Enemy.DetermineTarget();
        if (target == null)
        {
            fsm.StateCanExit();
            return;
        }
        else
        {
            this.Target = target.transform;
        }

        Agent.enabled = true;
        Agent.isStopped = false;
        // Animator.Play("Walk");

        // var propertyBlock = new MaterialPropertyBlock();
        // propertyBlock.SetColor("_Color", Color.cyan);
        // Enemy.MeshRenderer.SetPropertyBlock(propertyBlock);
    }

    public override void OnLogic()
    {
        base.OnLogic();

        if (Target == null)
        {
            fsm.StateCanExit();
            return;
        }

        if (!RequestedExit)
        {
            // you can add a more complex movement prediction algorithm like what 
            // we did in AI Series 44: https://youtu.be/1Jkg8cKLsC0

            Vector3 normDir = (Target.position - Enemy.transform.position).normalized;

            Agent.SetDestination(Enemy.transform.position - (normDir * 4f));
            Debug.DrawLine(Enemy.transform.position, Enemy.transform.position - (normDir * 4f), Color.red);
        }
        else if (Agent.remainingDistance <= Agent.stoppingDistance)
        {
            // In case that we were requested to exit, we will continue moving to the last known position prior to transitioning out to idle.
            fsm.StateCanExit();
        }
    }
}

