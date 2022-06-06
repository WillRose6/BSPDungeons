using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrapDoor : AnimatedInteractableObject
{
    public ItemTemplate neededItem;

    public override void Interact()
    {
        if (player.inventory.ContainsObject(neededItem))
        {
            Open();
        }
        else
        {
            GameObject.FindGameObjectWithTag("UI").GetComponent<DungeonUI>().ShowNotification("Looks like you need a key...", true);
        }
    }

    public void Open()
    {
        anim.SetTrigger("Open");
        Collider[] colliders = GetComponents<Collider>();
        foreach(Collider c in colliders)
        {
            c.enabled = false;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            NextLevel();
        }
    }

    public void NextLevel()
    {
        List<Item> itemsToRemove = new List<Item>();
        foreach(Item i in player.inventory.GetItems(ItemTemplate.ItemType.Consumable))
        {
            if(References.instance.GetItemTemplateByID(i.TemplateID).spawnType == ItemTemplate.SpawnType.OnePerLevel)
            {
                itemsToRemove.Add(i);
            }
        }

        for(int i = 0; i < itemsToRemove.Count; i++)
        {
            player.inventory.RemoveItemFromInventory(itemsToRemove[i], ItemTemplate.ItemType.Consumable);
        }

        GameSerializer.instance.SaveGame();
        WaitForFrame();
        GameManager.instance.NextLevel();
    }

    public IEnumerator WaitForFrame()
    {
        yield return null;
    }

    public void FreezePlayer()
    {
        player.FreezePlayer();
        player.HideMesh();
    }
}
