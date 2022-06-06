using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSelectionUI : WeaponFocusInventoryComponent
{
    public List<Image> itemImages;
    public List<Image> weaponImages;
    public List<Image> equippedImages;
    public List<Image> upgradeImages;
    private int upgradeCounter = 0;
    private ItemDisplay currentDisplay;
    public Image xpBar;
    private int slot;

    public override void OpenItemSelection(Weapon equipped, ItemDisplay display, int slot)
    {
        FindPlayer();
        base.OpenItemSelection(equipped, display, slot);

        this.slot = slot;
        LoadItems(itemImages, ItemTemplate.ItemType.WeaponUpgrade);
        LoadWeapons(weaponImages);
        RecalculateXPBar(equipped);
        DisplayEquippedSymbols();

        ReloadImages();
        currentDisplay = display;
    }

    public void RecalculateXPBar(Weapon equipped)
    {
        xpBar.fillAmount = ((float)equipped.xp / (float)equipped.requiredXP);
    }

    public void ReloadImages()
    {
        foreach (Image i in upgradeImages)
        {
            ResetSlot(i, i.transform.parent.GetComponent<Image>());
        }
        upgradeCounter = 0;

        for (int i = 0; i < equippedWeapon.equippedItems.Count; i++)
        {
            WeaponUpgradeItemTemplate it = (WeaponUpgradeItemTemplate)(References.instance.GetItemTemplateByID(equippedWeapon.equippedItems[i].TemplateID));
            SetItemToSlot(equippedWeapon.equippedItems[i], upgradeImages[i], upgradeImages[i].transform.parent.GetComponent<Image>(), it);
            upgradeCounter++;
        }
    }

    public override Item GetItem(int index)
    {
        return player.inventory.GetItem(index, ItemTemplate.ItemType.WeaponUpgrade);
    }

    public void AddItemToWeapon(int index)
    {
        if (index < player.inventory.GetAmountOfItemsInInventory(ItemTemplate.ItemType.WeaponUpgrade))
        {
            if (upgradeCounter < upgradeImages.Count)
            {
                Item item = GetItem(index);
                equippedWeapon.equippedItems.Add(item);
                WeaponUpgradeItemTemplate it = (WeaponUpgradeItemTemplate)(References.instance.GetItemTemplateByID(item.TemplateID));
                SetItemToSlot(item, upgradeImages[upgradeCounter], upgradeImages[upgradeCounter].transform.parent.GetComponent<Image>(), it);
                player.inventory.RemoveItemFromInventory(item, ItemTemplate.ItemType.WeaponUpgrade);
                LoadItems(itemImages, ItemTemplate.ItemType.WeaponUpgrade);
                upgradeCounter++;

                for (int i = 0; i < it.stats.Length; i++)
                {
                    TextMeshProUGUI text = statTexts[(int)it.stats[i].typeOfStat];
                    equippedWeapon.stats[(int)it.stats[i].typeOfStat].value = (equippedWeapon.stats[(int)it.stats[i].typeOfStat].value + it.stats[i].value);
                    text.text = (equippedWeapon.stats[(int)it.stats[i].typeOfStat].value).ToString();
                }
            }
        }
    }

    public void ShowItemIncreases(int index)
    {
        if (index < player.inventory.GetAmountOfItemsInInventory(ItemTemplate.ItemType.WeaponUpgrade))
        {
            Item item = GetItem(index);
            WeaponUpgradeItemTemplate it = (WeaponUpgradeItemTemplate)(References.instance.GetItemTemplateByID(item.TemplateID));
            for (int i = 0; i < it.stats.Length; i++)
            {
                TextMeshProUGUI text = statTexts[(int)it.stats[i].typeOfStat];
                text.text = (equippedWeapon.stats[(int)it.stats[i].typeOfStat].value + it.stats[i].value).ToString();
                text.color = Color.green;
            }
        }
    }

    public void HideItemIncreases()
    {
        for (int i = 0; i < statTexts.Length; i++)
        {
            statTexts[i].color = Color.white;
            statTexts[i].text = equippedWeapon.stats[i].value.ToString();
        }
    }

    public void RemoveItemFromWeapon(int index)
    {
        if (index < equippedWeapon.equippedItems.Count)
        {
            Item item = equippedWeapon.equippedItems[index];
            WeaponUpgradeItemTemplate it = (WeaponUpgradeItemTemplate)(References.instance.GetItemTemplateByID(item.TemplateID));
            equippedWeapon.equippedItems.Remove(item);
            player.inventory.AddToInventory(it, false);
            LoadItems(itemImages, ItemTemplate.ItemType.WeaponUpgrade);
            ReloadImages();

            for (int i = 0; i < it.stats.Length; i++)
            {
                TextMeshProUGUI text = statTexts[(int)it.stats[i].typeOfStat];
                equippedWeapon.stats[(int)it.stats[i].typeOfStat].value = (equippedWeapon.stats[(int)it.stats[i].typeOfStat].value - it.stats[i].value);
                text.text = (equippedWeapon.stats[(int)it.stats[i].typeOfStat].value).ToString();
            }

            for (int i = 0; i < statTexts.Length; i++)
            {
                statTexts[i].text = equippedWeapon.stats[i].value.ToString();
            }
        }
    }

    public void UpgradeEquippedWeapon()
    {
        if(equippedWeapon.equippedItems.Count > 0)
        {
            if (equippedWeapon.xp >= equippedWeapon.requiredXP)
            {
                equippedWeapon.ChangeLevel(1);
                equippedWeapon.equippedItems = new List<Item>();
                foreach (Image i in upgradeImages)
                {
                    ResetSlot(i, i.transform.parent.GetComponent<Image>());
                }
                upgradeCounter = 0;
                equippedWeaponNameText.text = equippedWeapon.Name;
                equippedWeapon.RecalculateXPNeeded();
                equippedWeapon.xp = 0;
                player.inventory.UpdateXPBars();
                RecalculateXPBar(equippedWeapon, xpBar);
            }
        }
    }

    public void OpenTransmutationScreen()
    {
        player.mainUI.ToggleTransmutationScreen();
        player.mainUI.transmutationScreen.OpenItemSelection(equippedWeapon, currentDisplay, slot);
    }

    public void EquipNewWeapon(int index)
    {
        Weapon w = player.inventory.GetWeapons()[index];
        foreach (Weapon equipped in player.inventory.EquippedWeapons)
        {
            if(w == equipped)
            {
                return;
            }
        }
        player.inventory.EquipNewWeapon(index, slot);
        OpenItemSelection(w, currentDisplay, slot);
        currentDisplay.SetItemForDisplay(w);
        DisplayEquippedSymbols();
    }

    public void DisplayEquippedSymbols()
    {
        foreach(Image i in equippedImages)
        {
            i.gameObject.SetActive(false);
        }

        foreach (Weapon equipped in player.inventory.EquippedWeapons)
        {
            int index = player.inventory.GetWeapons().FindIndex(item => item == equipped);
            equippedImages[index].gameObject.SetActive(true);
        }
    }

    public override void Back()
    {
        base.Back();
        player.mainUI.ToggleInventory(true);
        gameObject.SetActive(false);
    }
}
