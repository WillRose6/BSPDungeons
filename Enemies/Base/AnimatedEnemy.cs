using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedEnemy : MovingEnemy
{
    public Animator anim;
    public AnimationClip[] hitAnimations;
    public List<AnimationClip> frozenAnimations;

    public override void Die()
    {
        base.Die();
        anim.SetTrigger("Death");
    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
        anim.CrossFade(hitAnimations[Random.Range(0, hitAnimations.Length)].name, 0.1f);
    }

    public bool CheckFrozen()
    {
        if (anim != null)
        {
            if (anim.GetCurrentAnimatorClipInfo(0).Length > 0)
            {
                AnimationClip clip = anim.GetCurrentAnimatorClipInfo(0)[0].clip;
                if (clip)
                {
                    return frozenAnimations.Contains(clip);

                }
            }
        }

        return false;
    }

    protected override void Start()
    {
        base.Start();
        InvokeRepeating("UpdateCanMove", 0.05f, 0.05f);
    }

    protected virtual void UpdateCanMove()
    {
        if (CheckFrozen())
        {
            canMove = false;
            canRotate = false;
            agent.isStopped = true;
            agent.velocity = Vector3.zero;

        }
        else
        {
            canMove = true;
            canRotate = true;
            if (agent)
            {
                agent.isStopped = false;

            }
        }
    }
}
