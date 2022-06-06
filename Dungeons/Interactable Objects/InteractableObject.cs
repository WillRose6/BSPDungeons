using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    protected Player player;

    public virtual void Interact()
    {
        Activate();
    }

    public virtual void Activate()
    {

    }

    public virtual bool CanInteract()
    {
        return true;
    }

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
}
