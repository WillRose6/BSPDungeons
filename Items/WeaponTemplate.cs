using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTemplate : ItemTemplate
{
    public Stat[] stats = new Stat[8];
    public float xpMultiplier = 1;
    public int requiredXPOffset = 50;
    public WeaponTemplate[] weaponTransmutations;
}

[System.Serializable]
public class Weapon : Item
{
    public Stat[] stats = new Stat[8];

    public List<Item> equippedItems;
    public int Level = 0;
    public string Name;
    public int xp = 0;
    public int requiredXP = 100;

    public Weapon(WeaponTemplate weapon) : base(weapon)
    {
        for(int i = 0; i < weapon.stats.Length; i++)
        {
            stats[i].typeOfStat = weapon.stats[i].typeOfStat;
            stats[i].value = weapon.stats[i].value;
            Level = 0;
            xp = 0;
            RecalculateXPNeeded();
            equippedItems = new List<Item>();
            SetNameToIncludeLevel();
        }
    }

    public Weapon(Weapon weapon) : base(References.instance.GetWeaponTemplateByID(weapon.TemplateID))
    {
        for (int i = 0; i < weapon.stats.Length; i++)
        {
            stats[i].typeOfStat = weapon.stats[i].typeOfStat;
            stats[i].value = weapon.stats[i].value;
            Level = weapon.Level;
            xp = weapon.xp;
            RecalculateXPNeeded();
            equippedItems = weapon.equippedItems;
            SetNameToIncludeLevel();
        }
    }

    public void ChangeLevel(int amount)
    {
        Level += amount;
        SetNameToIncludeLevel();
    }

    public void SetNameToIncludeLevel()
    {
        if (Level != 0)
        {
            Name = References.instance.GetWeaponTemplateByID(TemplateID).Name + " +" + Level;
        }
        else
        {
            Name = References.instance.GetWeaponTemplateByID(TemplateID).Name;
        }
    }

    public void ReceiveXP(int XP)
    { 
        xp = Mathf.Clamp(xp+XP, 0, requiredXP);
    }

    public void RecalculateXPNeeded()
    {
        requiredXP = References.instance.GetWeaponTemplateByID(TemplateID).requiredXPOffset;
        requiredXP += Mathf.CeilToInt(((Level + 1) * References.instance.GetWeaponTemplateByID(TemplateID).xpMultiplier) * 100);
    }
}

[System.Serializable]
public struct Stat
{
    public enum StatType
    {
        Damage,
        AttackSpeed,
        CriticalChance,
        CriticalDamage,
        Luck,
        MagicPower,
        Slow,
        LifeSteal,
    }

    public StatType typeOfStat;
    public int value;
}
