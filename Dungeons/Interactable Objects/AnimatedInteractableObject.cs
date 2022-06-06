using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedInteractableObject : SituationalInteractableObject
{
    public Animator anim;

    protected override void Start()
    {
        base.Start();
        if (!anim)
        {
            anim = GetComponent<Animator>();
        }
    }

    public override void Activate()
    {
        base.Activate();
        anim.SetTrigger("Interact");
    }
}
