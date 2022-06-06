using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : AnimatedInteractableObject
{
    public GameObject itemPrefab;
    public Transform itemPosition;
    private GameObject heldItem;

    public override void Activate()
    {
        base.Activate();
        ItemPickUp i = Instantiate(itemPrefab, itemPosition.transform.position, itemPosition.transform.rotation).GetComponent<ItemPickUp>();
        i.SetItem();
        gameObject.tag = "IgnoreInteraction";
    }

    public override bool CanInteract()
    {
        float dot = Vector3.Dot(player.transform.position - transform.position, transform.forward);
        if(dot > 0.7f)
        {
            return true;
        }

        return false;
    }
}
