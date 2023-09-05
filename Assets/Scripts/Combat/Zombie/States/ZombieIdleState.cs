using UnityEngine;
using Lix.Core;

public class ZombieIdleState : EnemyStateBase
{
    private Player _player;
    public ZombieIdleState(bool needsExitTime, Zombie Zombie) : base(needsExitTime, Zombie)
    {
        _player = ServiceLocator.Get<Player>(true);
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

        if (_player == null)
        {
            _player = ServiceLocator.Get<Player>(true);
            if (_player == null)
            {
                base.OnLogic();
                return;
            }
        }

        Agent.SetDestination(_player.transform.position);

        base.OnLogic();
    }
}

