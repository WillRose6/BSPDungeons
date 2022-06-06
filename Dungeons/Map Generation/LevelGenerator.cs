using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public MapGenerator mapGenerator;
    public DetailGenerator detailGenerator;

    void Awake()
    {
        mapGenerator.GenerateMap();
        detailGenerator.SpawnDetails(mapGenerator.GetSubDungeons());
    }
}
