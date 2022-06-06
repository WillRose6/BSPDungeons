using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private List<Item> collectedConsumables = new List<Item>();
    [SerializeField]
    private List<Item> collectedItemUpgrades = new List<Item>();
    [SerializeField]
    private List<Weapon> weapons = new List<Weapon>();
    private Weapon[] equippedWeapons = new Weapon[2];
    public Weapon[] EquippedWeapons
    {
        get { return equippedWeapons; }
        set { equippedWeapons = value; }
    }
    private Player player;

    public void EquipStartingWeapons()
    {
        if(weapons.Count > 0)
        {
            for (int i = 0; i < Mathf.Min(2, weapons.Count); i++)
            {
                EquippedWeapons[i] = weapons[i];
            }
        }

        UpdateXPBars();
    }

    public Weapon LeftHandWeapon()
    {
        return EquippedWeapons[0];
    }

    public Weapon RightHandWeapon()
    {
        return EquippedWeapons[1];
    }

    public void AddToInventory(ItemTemplate item, bool notify)
    {
        Item i = new Item(item);
        if (item.itemType == ItemTemplate.ItemType.WeaponUpgrade)
        {
            collectedItemUpgrades.Add(i);
        }
        else
        {
            collectedConsumables.Add(i);
        }
        if (notify)
        {
            player.mainUI.ShowItem(item);
        }
    }

    public void AddToInventory(WeaponTemplate template, bool notify)
    {
        Weapon w = new Weapon(template);
        weapons.Add(w);
        if (notify)
        {
            player.mainUI.ShowItem(template);
        }
        EquipStartingWeapons();
        player.mainUI.transmutationScreen.unlockedWeapons.Add(w);
    }

    public void AddToInventory(Item item, bool notify)
    {
        if (References.instance.GetItemTemplateByID(item.TemplateID).itemType == ItemTemplate.ItemType.WeaponUpgrade)
        {
            collectedItemUpgrades.Add(item);
        }
        else
        {
            collectedConsumables.Add(item);
        }
        if (notify)
        {
            player.mainUI.ShowItem(item);
        }
    }

    public void AddToInventory(Weapon weapon, bool notify)
    {
        weapons.Add(weapon);
        if (notify)
        {
            player.mainUI.ShowItem(weapon);
        }
        EquipStartingWeapons();
        player.mainUI.transmutationScreen.unlockedWeapons.Add(weapon);
    }

    public bool ContainsObject(Weapon weapon)
    {
        return weapons.Exists(item => item == weapon);
    }

    public bool ContainsObject(Item item, ItemTemplate.ItemType typeOfItem)
    {
        if (typeOfItem == ItemTemplate.ItemType.WeaponUpgrade)
        {
            return collectedItemUpgrades.Contains(item);
        }
        else
        {
            return collectedConsumables.Contains(item);
        }
    }

    public bool ContainsObject(ItemTemplate template)
    {
        if (template.itemType == ItemTemplate.ItemType.WeaponUpgrade)
        {
            return collectedItemUpgrades.Exists(item => (item.TemplateID == template.ID));
        }
        else
        {
            return collectedConsumables.Exists(item => (item.TemplateID == template.ID));
        }
    }

    public Item GetItem(int index, ItemTemplate.ItemType type)
    {
        if (type == ItemTemplate.ItemType.WeaponUpgrade)
        {
            if (index > -1 && index < collectedItemUpgrades.Count)
            {
                return collectedItemUpgrades[index];
            }
        }
        else
        {
            if (index > -1 && index < collectedConsumables.Count)
            {
                return collectedConsumables[index];
            }
        }

        return null;
    }

    public Weapon GetWeapon(int index)
    {
        if (index > -1 && index < weapons.Count)
        {
            return weapons[index];
        }
        else
        {
            return null;
        }
    }

    public int GetAmountOfWeaponsInInventory()
    {
        return weapons.Count;
    }

    public int GetAmountOfItemsInInventory(ItemTemplate.ItemType typeOfItem)
    {
        if (typeOfItem == ItemTemplate.ItemType.WeaponUpgrade)
        {
            return collectedItemUpgrades.Count;
        }
        else
        {
            return collectedConsumables.Count;
        }
    }

    public List<Weapon> GetWeapons()
    {
        return weapons;
    }

    public List<Item> GetItems(ItemTemplate.ItemType typeOfItem)
    {
        if(typeOfItem == ItemTemplate.ItemType.WeaponUpgrade)
        {
            return collectedItemUpgrades;
        }
        else
        {
            return collectedConsumables;
        }
    }

    public void RemoveItemFromInventory(Item item, ItemTemplate.ItemType typeOfItem)
    {
        if(typeOfItem == ItemTemplate.ItemType.WeaponUpgrade)
        {
            collectedItemUpgrades.Remove(item);
        }
        else
        {
            collectedConsumables.Remove(item);
        }
    }

    public void RemoveWeaponFromInventory(Weapon weapon)
    {
        weapons.Remove(weapon);
    }

    public void ResetInventory()
    {
        collectedConsumables = new List<Item>();
        collectedItemUpgrades = new List<Item>();
        weapons = new List<Weapon>();
    }

    public void RecieveWeaponXP(int xp)
    {
        for(int i = 0; i < EquippedWeapons.Length; i++)
        {
            if(EquippedWeapons[i] != null)
            {
                EquippedWeapons[i].ReceiveXP(xp);
                UpdateXPBars();
            }
        }
    }

    public void UpdateXPBars()
    {
        if (player)
        {
            if (player.GetComponent<DungeonPlayer>())
            {
                for (int i = 0; i < EquippedWeapons.Length; i++)
                {
                    if (EquippedWeapons[i] != null)
                    {
                        player.mainUI.RecalculateXPBar(EquippedWeapons[i], ((DungeonPlayer)player).ui.xpBars[i]);
                    }
                }
            }
        }
    }

    public void EquipNewWeapon(int index, int slot)
    {
        EquippedWeapons[slot] = weapons[index];
        UpdateXPBars();
    }

    private void Start()
    {
        player = GetComponent<Player>();
    }
}
