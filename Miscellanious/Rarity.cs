using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Items", menuName = "Items/Rarity", order = 1)]
public class Rarity : ScriptableObject
{ 
    public enum Level
    {
        Common,
        Rare,
        UltraRare,
        Legendary,
        Mythical,
    }

    public Level level;
    public Color colour;
}
