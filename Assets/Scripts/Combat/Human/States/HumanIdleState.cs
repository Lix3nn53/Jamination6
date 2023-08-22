using UnityEngine;

public class HumanIdleState : EnemyStateBase
{
    private float AnimationLoopCount = 0;

    public HumanIdleState(bool needsExitTime, Human Human) : base(needsExitTime, Human) { }

    public override void OnEnter()
    {
        base.OnEnter();
        Agent.isStopped = true;
        Animator.Play("Idle_A");

        var propertyBlock = new MaterialPropertyBlock();
        propertyBlock.SetColor("_Color", Color.blue);
        Enemy.MeshRenderer.SetPropertyBlock(propertyBlock);
    }

    public override void OnLogic()
    {
        AnimatorStateInfo state = Animator.GetCurrentAnimatorStateInfo(0);

        if (state.normalizedTime >= AnimationLoopCount + 1)
        {
            float value = Random.value;
            if (value < 0.95f)
            {
                if (!state.IsName("Idle_A"))
                {
                    AnimationLoopCount = 0;
                }
                else
                {
                    AnimationLoopCount++;
                }
                Animator.Play("Idle_A");
            }
            else if (value < 0.975f)
            {
                if (!state.IsName("Idle_B"))
                {
                    AnimationLoopCount = 0;
                }
                else
                {
                    AnimationLoopCount++;
                }
                Animator.Play("Idle_B");
            }
            else
            {
                if (!state.IsName("Idle_C"))
                {
                    AnimationLoopCount = 0;
                }
                else
                {
                    AnimationLoopCount++;
                }
                Animator.Play("Idle_C");
            }
        }

        base.OnLogic();
    }
}

