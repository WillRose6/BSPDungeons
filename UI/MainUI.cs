using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MainUI : UIInventoryComponent
{
    [Header("Notifications")]
    public TextMeshProUGUI notificationText;
    public GameObject notificationWindow;

    [Header("Items")]
    public GameObject ItemsHolder;
    public GameObject itemDisplayPrefab;

    [Header("Inventory")]
    public GameObject inventoryHolderObject;
    public GameObject inventoryObject;
    public TextMeshProUGUI PlayerName;
    public TextMeshProUGUI PlayerStatus;
    public TextMeshProUGUI DamageAmountText;
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI AttemptText;
    public List<Image> itemImages;
    public TextMeshProUGUI LeftHandWeaponText;
    public TextMeshProUGUI RightHandWeaponText;
    public ItemDisplay leftWeaponDisplay;
    public ItemDisplay rightWeaponDisplay;

    public ItemSelectionUI itemSelectionScreen;
    public TransmutationUI transmutationScreen;

    [Header("Console")]
    public GameObject Console;

    public virtual void Begin(float StartHealth)
    {

    }

    public void ShowItem(Item item)
    {
        GameObject g = Instantiate(itemDisplayPrefab, ItemsHolder.transform);
        g.GetComponentInChildren<TextMeshProUGUI>().text = References.instance.GetItemTemplateByID(item.TemplateID).Name;
        g.GetComponentInChildren<Image>().sprite = References.instance.GetItemTemplateByID(item.TemplateID).sprite;
        g.GetComponentInChildren<RawImage>().color = References.instance.GetItemTemplateByID(item.TemplateID).rarity.colour;
        Destroy(g, 4.0f);
    }

    public void ShowItem(ItemTemplate item)
    {
        GameObject g = Instantiate(itemDisplayPrefab, ItemsHolder.transform);
        g.GetComponentInChildren<TextMeshProUGUI>().text = item.Name;
        g.GetComponentInChildren<Image>().sprite = item.sprite;
        g.GetComponentInChildren<RawImage>().color = item.rarity.colour;
        Destroy(g, 4.0f);
    }

    public void ShowNotification(string text, bool priority)
    {
        if (priority == false)
        {
            if (notificationWindow.activeInHierarchy)
            {
                return;
            }
        }

        notificationWindow.SetActive(true);
        notificationText.text = text;
    }

    public void CloseNotification()
    {
        notificationWindow.SetActive(false);
    }

    protected override void Start()
    {
        base.Start();
        ClearCurrentlySelectedItem();
    }

    public void ToggleInventory(bool toggle)
    {
        if (toggle)
        {
            SetupInventoryInformation();
        }
        for(int i = 0; i < inventoryHolderObject.transform.childCount; i++)
        {
            inventoryHolderObject.transform.GetChild(i).gameObject.SetActive(false);
        }
        inventoryObject.SetActive(true);
        inventoryHolderObject.SetActive(toggle);
    }

    public void SetupInventoryInformation()
    {
        PlayerName.text = Player.playerName;
        LevelText.text = GameManager.level.ToString();
        AttemptText.text = GameManager.attempts.ToString();
        DamageAmountText.text = (player.inventory.LeftHandWeapon().stats[0].value + player.inventory.RightHandWeapon().stats[0].value).ToString();

        LoadItems(itemImages, ItemTemplate.ItemType.Consumable);
        Inventory inv = player.inventory;

        leftWeaponDisplay.SetItemForDisplay(inv.LeftHandWeapon());
        rightWeaponDisplay.SetItemForDisplay(inv.RightHandWeapon());
        LeftHandWeaponText.text = inv.LeftHandWeapon().Name;
        RightHandWeaponText.text = inv.RightHandWeapon().Name;
    }

    //Horrible use of int - enum or basically anything would be better but I can't because unity buttons don't support it :(
    public void ShowItemSelectionScreen(int selection)
    {
        itemSelectionScreen.gameObject.SetActive(true);
        ItemSelectionUI i = itemSelectionScreen.GetComponent<ItemSelectionUI>();
        switch (selection)
        {
            case 0:
                i.OpenItemSelection(player.inventory.LeftHandWeapon(), leftWeaponDisplay, 0);
                break;

            case 1:
                i.OpenItemSelection(player.inventory.RightHandWeapon(), rightWeaponDisplay, 1);
                break;

            default:
                Debug.LogError("Had to use default option when selecting item.");
                i.OpenItemSelection(player.inventory.LeftHandWeapon(), leftWeaponDisplay, 0);
                break;
        }

        inventoryObject.SetActive(false);
    }

    public void ToggleConsole(bool toggle)
    {
        Console.SetActive(toggle);
    }

    public void ToggleTransmutationScreen()
    {
        bool toggle = !transmutationScreen.gameObject.activeInHierarchy;
        inventoryObject.gameObject.SetActive(!toggle);
        itemSelectionScreen.gameObject.SetActive(!toggle);
        transmutationScreen.gameObject.SetActive(toggle);
    }
}