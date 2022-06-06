using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DeveloperConsole : MonoBehaviour
{
    private InputField inputField;
    public Command[] commands;
    int parameter = 0;
    private Player player;
    private MainUI ui;

    public void Start()
    {
        if (!inputField)
        {
            inputField = GetComponentInChildren<InputField>();
        }
        ui = GameObject.FindGameObjectWithTag("UI").GetComponent<MainUI>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void Update()
    {
        if (Input.GetButtonDown("Enter"))
        {
            EvaluateCommand(inputField.text);
        }
    }

    public void EvaluateCommand(string input)
    {
        bool record = false;
        string command = "";
        string s_parameter = "";
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == ' ')
            {
                record = true;
                continue;
            }
            else if(!record)
            {
                command += input[i];
            }
            if (record)
            {
                s_parameter += int.Parse(input[i].ToString());
            }
        }

        if (s_parameter != "")
        {
            parameter = int.Parse(s_parameter);
        }

        foreach (Command c in commands)
        {
            if(c.requiredText == command)
            {
                c.action.Invoke();
            }
        }

        parameter = 0;
        inputField.text = "";
    }

    public void AddItem()
    {
        player.inventory.AddToInventory(References.instance.GetItemTemplateByID(parameter), true);
    }

    public void AddWeapon()
    {
        player.inventory.AddToInventory(References.instance.GetWeaponTemplateByID(parameter), true);
    }

    public void AddItem(int id)
    {
        player.inventory.AddToInventory(References.instance.GetItemTemplateByID(id), true);
    }

    public void AddWeapon(int id)
    {
        player.inventory.AddToInventory(References.instance.GetWeaponTemplateByID(id), true);
    }

    public void ClearInventory()
    {
        player.inventory.ResetInventory();
        ui.transmutationScreen.ClearUnlockedWeapons();
        AddWeapon(100);
        AddWeapon(100);
    }
}

[System.Serializable]
public struct Command
{
    public string requiredText;
    public UnityEvent action;
}
