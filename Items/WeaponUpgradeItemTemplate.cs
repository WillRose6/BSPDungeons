using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUpgradeItemTemplate : ItemTemplate
{
    public Stat[] stats;

    private void Awake()
    {
        itemType = ItemType.WeaponUpgrade;
    }
}
