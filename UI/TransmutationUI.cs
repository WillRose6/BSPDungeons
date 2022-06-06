using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TransmutationUI : WeaponFocusInventoryComponent, IDragHandler
{
    public RectTransform graph;
    public WeaponTree weaponTree;
    public WeaponTemplate startingWeapon;
    public List<Weapon> unlockedWeapons;

    public override void OpenItemSelection(Weapon equipped, ItemDisplay display, int slot)
    {
        base.OpenItemSelection(equipped, display, slot);
        weaponTree.CreateTree(startingWeapon);
    }

    public void OnDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        graph.transform.position += (Vector3)eventData.delta;
    }

    public void CompareWeapon(WeaponTemplate template)
    {
        Weapon w = player.mainUI.itemSelectionScreen.equippedWeapon;
        for(int i = 0; i < w.stats.Length; i++)
        {
            if(template.stats[i].typeOfStat == w.stats[i].typeOfStat)
            {
                if(template.stats[i].value > w.stats[i].value)
                {
                    statTexts[i].color = Color.red;
                    statTexts[i].text = template.stats[i].value.ToString();
                }
            }
        }
    }

    public void UnCompareWeapon()
    {
        Weapon w = player.mainUI.itemSelectionScreen.equippedWeapon;
        for (int i = 0; i < w.stats.Length; i++)
        {
            statTexts[i].color = Color.white;
            statTexts[i].text = w.stats[i].value.ToString();
        }
    }

    public bool CheckIfWeaponIsUpgradable(WeaponTemplate template)
    {
        int counter = 0;
        Weapon w = player.mainUI.itemSelectionScreen.equippedWeapon;
        for(int i = 0; i < w.stats.Length; i++)
        {
            if(template.stats[i].typeOfStat == w.stats[i].typeOfStat)
            {
                if(template.stats[i].value <= w.stats[i].value)
                {
                    counter++;
                }
            }
        }

        return counter >= template.stats.Length;
    }

    public void UpgradeWeapon(WeaponTemplate template)
    {
        player.inventory.RemoveWeaponFromInventory(player.mainUI.transmutationScreen.equippedWeapon);
        player.inventory.AddToInventory(template, true);
        player.ToggleInventory(false);
        player.ToggleInventory(true);
    }

    public void ClearUnlockedWeapons()
    {
        unlockedWeapons = new List<Weapon>();
    }

    public override void Back()
    {
        base.Back();
        Weapon w = player.mainUI.itemSelectionScreen.equippedWeapon;
        int index = System.Array.IndexOf(player.inventory.EquippedWeapons, w);
        player.mainUI.ShowItemSelectionScreen(index);
        gameObject.SetActive(false);
    }
}
