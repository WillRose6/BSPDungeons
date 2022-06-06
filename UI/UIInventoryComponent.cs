using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInventoryComponent : MonoBehaviour
{
    [SerializeField]
    private Sprite emptySprite;
    public TextMeshProUGUI itemNameDescriptionText;
    public TextMeshProUGUI weaponNameDescriptionText;
    [SerializeField]
    private Color originalSlotColor = Color.white;
    protected Player player;

    protected virtual void Start()
    {
        FindPlayer();
    }

    public void FindPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void LoadItems(List<Image> slots, ItemTemplate.ItemType typeOfItem)
    {
        Inventory inv = player.inventory;

        for (int i = 0; i < inv.GetAmountOfItemsInInventory(typeOfItem); i++)
        {
            SetItemToSlot(inv.GetItem(i, typeOfItem), slots[i], slots[i].transform.parent.GetComponent<Image>());
        }
        for (int i = inv.GetAmountOfItemsInInventory(typeOfItem); i < slots.Count; i++)
        {
            slots[i].sprite = emptySprite;
            slots[i].transform.parent.GetComponent<Image>().color = originalSlotColor;
        }
    }

    public void LoadWeapons(List<Image> slots)
    {
        Inventory inv = player.inventory;

        for (int i = 0; i < inv.GetAmountOfWeaponsInInventory(); i++)
        {
            SetWeaponToSlot(inv.GetWeapon(i), slots[i], slots[i].transform.parent.GetComponent<Image>());
        }
        for (int i = inv.GetAmountOfWeaponsInInventory(); i < slots.Count; i++)
        {
            slots[i].sprite = emptySprite;
            slots[i].transform.parent.GetComponent<Image>().color = originalSlotColor;
        }
    }

    public void UpdateCurrentlySelectedItem(int index)
    {
        Item i = GetItem(index);
        if (i != null)
        {
            itemNameDescriptionText.text = References.instance.GetItemTemplateByID(i.TemplateID).Name + ": " + References.instance.GetItemTemplateByID(i.TemplateID).Description;
        }
        else
        {
            ClearCurrentlySelectedItem();
        }
    }

    public virtual Item GetItem(int index)
    {
        return player.inventory.GetItem(index, ItemTemplate.ItemType.Consumable);
    }

    public void ClearCurrentlySelectedItem()
    {

        itemNameDescriptionText.text = "";
    }

    public void UpdateCurrentlySelectedWeapon(int index)
    {
        Weapon i = player.inventory.GetWeapon(index);
        if (i != null)
        {
            WeaponTemplate template = References.instance.GetWeaponTemplateByID(i.TemplateID);
            weaponNameDescriptionText.text = i.Name + ": " + template.Description;
        }
        else
        {
            ClearCurrentlySelectedWeapon();
        }
    }

    public void ClearCurrentlySelectedWeapon()
    {

        weaponNameDescriptionText.text = "";
    }

    public void RecalculateXPBar(Weapon equipped, Image xpBar)
    {
        xpBar.fillAmount = ((float)equipped.xp / (float)equipped.requiredXP);
    }

    public virtual void Back()
    {

    }

    public virtual void SetItemToSlot(Item item, Image spriteSlot, Image colorSlot)
    {
        ItemTemplate it = (References.instance.GetItemTemplateByID(item.TemplateID));
        SetItemToSlot(item, spriteSlot, colorSlot, it);
    }

    public virtual void SetItemToSlot(Item item, Image spriteSlot, Image colorSlot, ItemTemplate it)
    {
        spriteSlot.sprite = it.sprite;
        colorSlot.color = it.rarity.colour;
    }

    public virtual void SetWeaponToSlot(Weapon weapon, Image spriteSlot, Image colorSlot)
    {
        WeaponTemplate it = References.instance.GetWeaponTemplateByID(weapon.TemplateID);
        SetWeaponToSlot(weapon, spriteSlot, colorSlot, it);
    }

    public virtual void SetWeaponToSlot(Weapon weapon, Image spriteSlot, Image colorSlot, WeaponTemplate it)
    {
        spriteSlot.sprite = it.sprite;
        colorSlot.color = it.rarity.colour;
    }

    public virtual void ResetSlot(Image spriteSlot, Image colorSlot)
    {
        spriteSlot.sprite = emptySprite;
        colorSlot.color = originalSlotColor;
    }
}
