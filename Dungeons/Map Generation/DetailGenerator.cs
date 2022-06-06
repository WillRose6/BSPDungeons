using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DetailGenerator : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject playerCameraPrefab;
    public GameObject trapDoor;

    [Header("Enemies")]
    public GameObject[] enemyPrefabs;
    public int minAmountOfEnemiesPerRoom, maxAmountOfEnemiesPerRoom;
    public float EnemySpawnScalar;

    [Header("Chests")]
    public GameObject chestPrefab;
    public int minAmountOfChestsPerRoom, maxAmountOfChestsPerRoom;
    public float ChestSpawnScalar;

    [Header("Traps")]
    public SpawnableTrap[] floorTraps;
    public SpawnableTrap[] roofTraps;
    public GameObject roofTrapHolder;
    public int minAmountOfFloorTrapsPerRoom, maxAmountOfFloorTrapsPerRoom;
    public int minAmountOfRoofTrapsPerRoom, maxAmountOfRoofTrapsPerRoom;
    public float floorTrapSpawnScalar;
    public float roofTrapSpawnScalar;

    [Header("Wall objects")]
    public SpawnableObject[] wallObjects;
    public int minAmountOfWallObjectsPerRoom, maxAmountOfWallObjectsPerRoom;

    [Header("Floor objects")]
    public SpawnableObject[] floorObjects;
    public int minAmountOfFloorObjectsPerRoom, maxAmountOfFloorObjectsPerRoom;

    private SubFloor startingDungeon;
    public NavMeshSurface navSurface;

    public void SpawnDetails(List<SubFloor> dungeons)
    {
        CreatePlayer(dungeons[Random.Range(0, dungeons.Count)]);
        CreateFloorTraps(dungeons);
        CreateRoomDetails(dungeons);
        CreateChests(dungeons);
        CreateRoofTraps(dungeons);
        CreateEnemies(dungeons);
        CreateTrapDoor(dungeons);
        navSurface.BuildNavMesh();
    }

    private void CreateRoomDetails(List<SubFloor> dungeons)
    {
        foreach (SubFloor s in dungeons)
        {
            Vector3 spawnPos = Vector3.zero;
            Quaternion rotation = Quaternion.identity;

            //Create the wall objects
            for (int i = 0; i < Random.Range(minAmountOfWallObjectsPerRoom, maxAmountOfWallObjectsPerRoom); i++)
            {
                SpawnableObject obj = wallObjects[Random.Range(0, wallObjects.Length)];
                for (int j = 0; j < Random.Range(obj.minMultiplier, obj.maxMultiplier); j++)
                {
                    switch (Random.Range(0, 4))
                    {
                        case 0:
                            spawnPos = new Vector3(Random.Range(s.room.x + 2, s.room.xMax - 2), 3.0f, s.room.y + 0.6f);
                            rotation = Quaternion.identity;
                            break;

                        case 1:
                            spawnPos = new Vector3(Random.Range(s.room.x + 2, s.room.xMax - 2), 3.0f, s.room.yMax - 1.6f);
                            rotation = Quaternion.Euler(0, 180, 0);
                            break;

                        case 2:
                            spawnPos = new Vector3(s.room.x + 0.6f, 3.0f, Random.Range(s.room.y + 2, s.room.yMax - 2));
                            rotation = Quaternion.Euler(0, 90, 0);
                            break;

                        case 3:
                            spawnPos = new Vector3(s.room.xMax - 1.6f, 3.0f, Random.Range(s.room.y + 2, s.room.yMax - 2));
                            rotation = Quaternion.Euler(0, -90, 0);
                            break;
                    }
                    Instantiate(obj.obj, spawnPos, rotation).transform.SetParent(transform);
                }
            }

            for (int i = 0; i < Random.Range(minAmountOfFloorObjectsPerRoom, maxAmountOfFloorObjectsPerRoom); i++)
            {
                SpawnableObject obj = floorObjects[Random.Range(0, floorObjects.Length)];
                for (int j = 0; j < Random.Range(obj.minMultiplier, obj.maxMultiplier); j++)
                {
                    rotation = Quaternion.Euler(0, Random.Range(0, 4) * 90, 0);
                    GameObject randomObj = s.GetRandomValidPointInRoom(true);
                    spawnPos = randomObj.transform.position;
                    Instantiate(obj.obj, spawnPos, rotation).transform.SetParent(transform);
                }
            }
        }
    }

    private void CreatePlayer(SubFloor startingPoint)
    {
        Instantiate(playerPrefab, new Vector3((startingPoint.room.xMax + startingPoint.room.x) / 2, 1.0f, (startingPoint.room.yMax + startingPoint.room.y) / 2), Quaternion.identity);
        startingDungeon = startingPoint;
    }

    private void CreateEnemies(List<SubFloor> dungeons)
    {
        foreach (SubFloor d in dungeons)
        {
            if (d != startingDungeon)
            {
                for (int i = 0; i < GetAmountOfObjectsToBuild(d, minAmountOfEnemiesPerRoom, maxAmountOfEnemiesPerRoom) * EnemySpawnScalar; i++)
                {
                    GameObject toUse = d.GetRandomValidPointInRoom(true);
                    if (toUse)
                    {
                        GameObject g = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], toUse.transform.position + new Vector3(0, 2f, 0), Quaternion.Euler(0, Random.Range(0, 360), 0));
                        g.transform.SetParent(transform);
                        g.GetComponent<Enemy>().subDungeon = d;
                    }
                }
            }
        }
    }

    private void CreateChests(List<SubFloor> dungeons)
    {
        foreach (SubFloor d in dungeons)
        {
            for (int i = 0; i < GetAmountOfObjectsToBuild(d, minAmountOfChestsPerRoom, maxAmountOfChestsPerRoom) * ChestSpawnScalar; i++)
            {
                GameObject toUse = d.GetRandomValidPointInRoom(true);
                if (toUse)
                {
                    Quaternion rotation = Quaternion.Euler(0, Random.Range(0, 3) * 90, 0);
                    Instantiate(chestPrefab, toUse.transform.position, rotation).transform.SetParent(transform);
                }
            }
        }
    }

    private void CreateFloorTraps(List<SubFloor> dungeons)
    {

        foreach (SubFloor d in dungeons)
        {
            if (d != startingDungeon)
            {
                for (int m = 0; m < GetAmountOfObjectsToBuild(d, minAmountOfFloorTrapsPerRoom, maxAmountOfFloorTrapsPerRoom); m++)
                {
                    SpawnableTrap spawnable = floorTraps[Random.Range(0, floorTraps.Length)];

                    for (int j = 0; j < (Random.Range(spawnable.minMultiplier, spawnable.maxMultiplier)); j++)
                    {
                        bool success = true;
                        List<GameObject> floorObjs = d.GetAreaInRoom(spawnable.width, spawnable.height, ref success);
                        if (success)
                        {
                            if (floorObjs != null && floorObjs.Count > 0)
                            {
                                Vector3 mid = Vector3.zero;
                                foreach (GameObject obj in floorObjs)
                                {
                                    mid += obj.transform.position;
                                }
                                mid /= floorObjs.Count;

                                GameObject g = Instantiate(spawnable.obj, mid, Quaternion.identity);
                                g.transform.SetParent(transform);
                                g.GetComponentInChildren<Trap>().subDungeon = d;
                                foreach (GameObject obj in floorObjs)
                                {
                                    Destroy(obj);
                                }

                                if (spawnable.activationType == SpawnableTrap.ActivationType.Trigger)
                                {
                                    GameObject floorObj = d.GetRandomValidPointInRoom(true);
                                    if (floorObj)
                                    {
                                        GameObject trigger = Instantiate(spawnable.trigger, floorObj.transform.position, Quaternion.Euler(0, Random.Range(0, 4) * 90, 0));
                                        trigger.transform.SetParent(transform);
                                        Destroy(floorObj);
                                        CollisionTriggerEventExecute executer = trigger.GetComponent<CollisionTriggerEventExecute>();
                                        Trap t = g.GetComponent<Trap>();
                                        executer.events.AddListener(delegate { t.ToggleActive(2.0f); });
                                    }
                                }

                                foreach (GameObject i in floorObjs)
                                {
                                    if (i)
                                    {
                                        d.RemoveFromList(i);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

    }

    private void CreateRoofTraps(List<SubFloor> dungeons)
    {
        foreach (SubFloor d in dungeons)
        {
            if (d != startingDungeon)
            {
                for (int i = 0; i < GetAmountOfObjectsToBuild(d, minAmountOfRoofTrapsPerRoom, maxAmountOfRoofTrapsPerRoom) * roofTrapSpawnScalar; i++)
                {
                    SpawnableTrap spawnable = roofTraps[Random.Range(0, roofTraps.Length)];
                    Vector3 spawnPos = Vector3.zero;
                    Quaternion rotation = Quaternion.identity;
                    Vector3 scale = Vector3.zero;

                    for (int j = 0; j < Random.Range(spawnable.minMultiplier, spawnable.maxMultiplier); j++)
                    {
                        if (d.room.size.x > d.room.size.y)
                        {
                            spawnPos = new Vector3(Random.Range(d.room.x + 2, d.room.xMax - 2), 5.0f, ((d.room.yMax + d.room.y) / 2) - 0.5f);
                            rotation = Quaternion.Euler(0, 90, 0);
                            scale = new Vector3((d.room.size.y - 1) / 2, 0.2f, 0.2f);
                        }
                        else
                        {
                            spawnPos = new Vector3(((d.room.xMax + d.room.x) / 2) - 0.5f, 5.0f, Random.Range(d.room.yMax - 2f, d.room.y + 2f));
                            rotation = Quaternion.identity;
                            scale = new Vector3((d.room.size.x - 1) / 2, 0.2f, 0.2f);
                        }
                        GameObject trap = Instantiate(spawnable.obj, spawnPos, rotation);
                        GameObject holder = Instantiate(roofTrapHolder, spawnPos, rotation);
                        holder.transform.localScale = scale;
                        trap.transform.SetParent(transform);
                        holder.transform.SetParent(transform);
                    }
                }
            }
        }
    }

    public void CreateTrapDoor(List<SubFloor> dungeons)
    {
        SubFloor d = null;
        GameObject pos = null;
        int count = 0;
        do
        {
            count++;
            if (count > 100)
            {
                break;
            }
            d = dungeons[Random.Range(0, dungeons.Count - 1)];
            pos = d.GetRandomValidPointInRoom(false);
        } while (pos == null);


        Instantiate(trapDoor, pos.transform.position, Quaternion.identity).transform.SetParent(transform);
        Destroy(pos);
    }

    public int GetAmountOfObjectsToBuild(SubFloor dungeon, int min, int max)
    {
        float scalar = ((dungeon.room.size.x * dungeon.room.size.y) / 100) - 1;
        int amountToSpawn = Mathf.Abs(Random.Range(Mathf.FloorToInt(scalar), Mathf.FloorToInt(scalar)));
        amountToSpawn = Mathf.Clamp(amountToSpawn, min, max);
        return amountToSpawn;
    }
}

[System.Serializable]
public class SpawnableObject
{
    public GameObject obj;
    public int minMultiplier, maxMultiplier;
}

[System.Serializable]
public class SpawnableTrap : SpawnableObject
{
    public enum ActivationType
    {
        None,
        Timed,
        Trigger,
    }

    public ActivationType activationType;
    public GameObject trigger;
    public int width = 1, height = 1;
}

