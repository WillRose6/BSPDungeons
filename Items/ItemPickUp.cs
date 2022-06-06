using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : InteractableObject
{
    public ItemTemplate heldItem;

    public void SetItem()
    {
        ItemTemplate item = null;
        do
        {
            item = References.instance.PossibleItems[Random.Range(0, References.instance.PossibleItems.Count)];
        }
        while (item.spawnType == ItemTemplate.SpawnType.OnePerLevel);

        heldItem = item;
    }

    public void SetItem(ItemTemplate[] items)
    {
        heldItem = items[Random.Range(0, items.Length - 1)];
    }

    public void SetItem(List<ItemTemplate> items)
    {
        heldItem = items[Random.Range(0, items.Count - 1)];
    }

    public void SetItem(ItemTemplate item)
    {
        heldItem = item;
    }

    public override void Interact()
    {
        base.Interact();
        player.inventory.AddToInventory(heldItem, true);
        Destroy(gameObject);
    }
}
