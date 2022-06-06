using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SituationalInteractableObject : InteractableObject
{
    public override void Interact()
    {
        if (CanInteract())
        {
            base.Interact();
        }
    }

    public override bool CanInteract()
    {
        return base.CanInteract();
    }
}
