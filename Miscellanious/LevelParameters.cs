using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelParameters", menuName = "Levels/LevelParameters", order = 1)]
public class LevelParameters : ScriptableObject
{
    public List<ItemTemplate> possibleItems;
    public List<WeaponTemplate> possibleWeapons;
    public List<Rarity> rarities;
    public List<AudioClip> songs;
}
