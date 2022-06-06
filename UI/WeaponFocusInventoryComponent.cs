using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponFocusInventoryComponent : UIInventoryComponent
{
    public RawImage equippedImage;
    public TextMeshProUGUI equippedWeaponNameText;
    public TextMeshProUGUI[] statTexts;
    public Weapon equippedWeapon;

    public virtual void OpenItemSelection(Weapon equipped, ItemDisplay display, int slot)
    {
        equippedWeapon = equipped;
        equippedImage.texture = display.GetComponentInChildren<Camera>().targetTexture;
        equippedWeaponNameText.text = equipped.Name;
        for (int i = 0; i < statTexts.Length; i++)
        {
            statTexts[i].text = equipped.stats[i].value.ToString();
        }
    }
}
