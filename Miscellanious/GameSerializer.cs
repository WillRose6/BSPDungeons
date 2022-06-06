using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameSerializer : MonoBehaviour
{
    public static GameSerializer instance;
    private Player player;
    private MainUI ui;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("More than one GameSaver in scene!");
            return;
        }

        instance = this;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        ui = GameObject.FindGameObjectWithTag("UI").GetComponent<MainUI>();
    }

    public void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, CreateSaveGame());
        file.Close();
    }

    public Save LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/gamesave.save")) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            if (file.Length > 0)
            {
                Save save = (Save)bf.Deserialize(file);
                file.Close();

                Inventory inventory = null;
                if (player)
                {
                    inventory = player.inventory;
                }
                for (int i = 0; i < save.items.Count; i++)
                {
                    inventory.AddToInventory(save.items[i], false);
                }

                for (int i = 0; i < save.weapons.Count; i++)
                {
                    Weapon w = new Weapon(save.weapons[i]);
                    inventory.AddToInventory(w, false);
                }

                foreach(Weapon w in save.unlockedWeapons)
                {
                    ui.transmutationScreen.unlockedWeapons.Add(w);
                }

                inventory.EquipStartingWeapons();

                return save;
            }
        }
        SaveGame();
        LoadGame();
        Debug.LogWarning("Could not find save!");
        return null;
    }

    private Save CreateSaveGame()
    {
        Save save = new Save();

        for (int i = 0; i < player.inventory.GetAmountOfItemsInInventory(ItemTemplate.ItemType.Consumable); i++)
        {
            save.items.Add(player.inventory.GetItem(i, ItemTemplate.ItemType.Consumable));
        }

        for (int i = 0; i < player.inventory.GetAmountOfItemsInInventory(ItemTemplate.ItemType.WeaponUpgrade); i++)
        {
            save.items.Add(player.inventory.GetItem(i, ItemTemplate.ItemType.WeaponUpgrade));
        }

        for (int i = 0; i < player.inventory.GetAmountOfWeaponsInInventory(); i++)
        {
            save.weapons.Add(player.inventory.GetWeapon(i));
        }

        foreach(Weapon w in ui.transmutationScreen.unlockedWeapons)
        {
            save.unlockedWeapons.Add(w);
        }
        return save;
    }
}

[System.Serializable]
public class Save
{
    public List<Item> items= new List<Item>();
    public List<Weapon> weapons = new List<Weapon>();
    public List<Weapon> unlockedWeapons = new List<Weapon>();
}
