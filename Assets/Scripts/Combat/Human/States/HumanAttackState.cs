using FSM;
using System;
using UnityEngine;

namespace LlamAcademy.FSM
{
    public class HumanAttackState : EnemyStateBase
    {
        public HumanAttackState(
            bool needsExitTime,
            Human Human,
            Action<State<EnemyState, EnemyStateEvent>> onEnter,
            float ExitTime = 0.33f) : base(needsExitTime, Human, ExitTime, onEnter) { }

        public override void OnEnter()
        {
            Agent.isStopped = true;
            base.OnEnter();
            Animator.Play("Attack");

            var propertyBlock = new MaterialPropertyBlock();
            propertyBlock.SetColor("_Color", Color.red);
            Enemy.MeshRenderer.SetPropertyBlock(propertyBlock);
        }
    }
}
