using UnityEngine;

public class HumanIdleState : EnemyStateBase
{
    private float _lastRandomAngleTime = Time.time;
    private float _randomAngleCooldown = 4f;
    private float _travelDistance = 8f;
    public HumanIdleState(bool needsExitTime, Human Human) : base(needsExitTime, Human)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();

        Agent.enabled = true;
        Agent.isStopped = false;
        // Animator.Play("Walk");

        // var propertyBlock = new MaterialPropertyBlock();
        // propertyBlock.SetColor("_Color", Color.cyan);
        // Enemy.MeshRenderer.SetPropertyBlock(propertyBlock);
    }

    public override void OnLogic()
    {
        // you can add a more complex movement prediction algorithm like what 
        // we did in AI Series 44: https://youtu.be/1Jkg8cKLsC0

        if (_lastRandomAngleTime + _randomAngleCooldown <= Time.time)
        {
            _lastRandomAngleTime = Time.time;
            // turn Enemy.transform to a random angle
            float angle = Random.Range(0f, 360f);

            Vector3 normDir = (Enemy.transform.forward + (Quaternion.Euler(0f, angle, 0f) * Vector3.forward)).normalized;

            Agent.SetDestination(Enemy.transform.position + (normDir * _travelDistance));
        }

        base.OnLogic();
    }
}

