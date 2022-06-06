using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class References : MonoBehaviour
{
    public System.Collections.Generic.List<ItemTemplate> PossibleItems
    {
        get { return levelParameters.possibleItems; }
    }
    public System.Collections.Generic.List<WeaponTemplate> PossibleWeapons
    {
        get { return levelParameters.possibleWeapons; }
    }
    public System.Collections.Generic.List<Rarity> Rarities
    {
        get { return levelParameters.rarities; }
    }
    public System.Collections.Generic.List<UnityEngine.AudioClip> Songs
    {
        get { return levelParameters.songs; }
    }
    public LevelParameters levelParameters;
    public GameObject itemPickUpPrefab;

    public static References instance;
    public static LevelParameters parameters;

    private void Awake()
    {
        if (instance)
        {
            Debug.LogError("More than one references object in scene! Please ensure that there is only one.");
            return;
        }

        instance = this;
    }

    public void SetupParameters(LevelParameters levelParameters)
    {
        this.levelParameters = levelParameters;
    }

    private void Start()
    {
        if (parameters)
        {
            SetupParameters(parameters);
        }
        if (levelParameters != null)
        {
            PossibleItems.Sort((a, b) => b.CompareTo(a));
            PossibleWeapons.Sort((a, b) => b.CompareTo(a));
        }
        else
        {
            Debug.LogError("Could not find level parameters!");
        }
    }

    public ItemTemplate GetItemTemplateByID(int ID)
    {
        ItemTemplate part = null;
        int min = 0;
        int N = PossibleItems.Count;
        int max = N - 1;
        do
        {
            int mid = (min + max) / 2;
            part = PossibleItems[mid];
            switch (part.CompareTo(ID))
            {
                case 1:
                    min = mid + 1;
                    break;

                case -1:max = mid - 1;
                    break;

                case 0:
                    return part;
            }
        } while (min <= max);
        return null;
    }

    public WeaponTemplate GetWeaponTemplateByID(int ID)
    {
        WeaponTemplate part = null;
        int min = 0;
        int N = PossibleWeapons.Count;
        int max = N - 1;
        do
        {
            int mid = (min + max) / 2;
            part = PossibleWeapons[mid];
            switch (part.CompareTo(ID))
            {
                case 1:
                    min = mid + 1;
                    break;

                case -1:
                    max = mid - 1;
                    break;

                case 0:
                    return part;
            }
        } while (min <= max);
        return null;
    }
}
