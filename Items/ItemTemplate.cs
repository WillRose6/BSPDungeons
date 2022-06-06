using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTemplate : MonoBehaviour, IComparable<ItemTemplate>
{
    public int ID;
    public string Name;
    public string Description;
    public Sprite sprite;
    public GameObject prefab;
    public Rarity rarity;
    public enum SpawnType
    {
        Constant,
        OnePerLevel,
    }
    public SpawnType spawnType;

    public enum ItemType
    {
        Consumable,
        WeaponUpgrade,
    }

    [HideInInspector]
    public ItemType itemType;

    public int CompareTo(ItemTemplate obj)
    {
        if(ID < obj.ID)
        {
            return 1;
        }
        else if(ID > obj.ID)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }

    public int CompareTo(int id)
    {
        if (ID < id)
        {
            return 1;
        }
        else if(ID > id)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }
}

[System.Serializable]
public class Item
{
    public int TemplateID;

    public Item(ItemTemplate template)
    {
        TemplateID = template.ID;
    }
}
