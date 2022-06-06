using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MapGenerator : MonoBehaviour
{
    public int rows, columns;
    public int minRoomSize, maxRoomSize;
    public GameObject floorObj;
    public GameObject corridorObj;
    public GameObject wallObj;
    public GameObject corridorWallObj;

    private GameObject[,] tilePositions;
    List<SubFloor> subDungeons = new List<SubFloor>();
    private DungeonPlayer player;

    public void GenerateMap()
    {
        SubFloor rootSubDungeon = new SubFloor(new Rect(0, 0, rows, columns));
        Partition(rootSubDungeon);
        rootSubDungeon.CreateRoom();

        tilePositions = new GameObject[rows, columns];
        CreateRooms(rootSubDungeon);
        CreateCorridors(rootSubDungeon);
        CreateWalls(rootSubDungeon);
        AddDungeonsToListReadyForDetails(rootSubDungeon);
    }

    public void Update()
    {
        foreach(SubFloor d in subDungeons)
        {
            if (player)
            {
                if(player.transform.position.x >= d.room.x && player.transform.position.x <= d.room.xMax && player.transform.position.z >= d.room.y && player.transform.position.z <= d.room.yMax)
                {
                    player.currentSubdungeon = d;
                }
            }
            else
            {
                player = GameObject.FindGameObjectWithTag("Player").GetComponent<DungeonPlayer>();
            }
        }
    }

    public void CreateWalls(SubFloor subDungeon)
    {
        if (subDungeon == null)
        {
            return;
        }
        if (subDungeon.AmALeaf())
        {
            for (int i = (int)subDungeon.room.x; i < subDungeon.room.xMax; i++)
            {
                for (int j = (int)subDungeon.room.y; j < subDungeon.room.yMax; j++)
                {
                    if (i == (int)subDungeon.room.x || i == (int)subDungeon.room.xMax - 1 || j == (int)subDungeon.room.y || j == (int)subDungeon.room.yMax - 1)
                    {
                        GameObject instance = null;
                        Vector2Int? position = GetPositionInArray(tilePositions[i, j]);

                        if (tilePositions[i, j])
                        {
                            subDungeon.RemoveFromList(tilePositions[i, j]);
                            if (tilePositions[position.Value.x - 1, position.Value.y])
                            {
                                if (tilePositions[position.Value.x - 1, position.Value.y].tag == "corridor")
                                {
                                    instance = Instantiate(corridorWallObj, new Vector3(i, 1f, j), Quaternion.identity) as GameObject;
                                    instance.transform.SetParent(transform);
                                    continue;
                                }
                            }

                            if (tilePositions[position.Value.x + 1, position.Value.y])
                            {
                                if (tilePositions[position.Value.x + 1, position.Value.y].tag == "corridor")
                                {
                                    instance = Instantiate(corridorWallObj, new Vector3(i, 1f, j), Quaternion.identity) as GameObject;
                                    instance.transform.SetParent(transform);
                                    continue;
                                }
                            }

                            if (tilePositions[position.Value.x, position.Value.y - 1])
                            {
                                if (tilePositions[position.Value.x, position.Value.y - 1].tag == "corridor")
                                {
                                    instance = Instantiate(corridorWallObj, new Vector3(i, 1f, j), Quaternion.identity) as GameObject;
                                    instance.transform.SetParent(transform);
                                    continue;
                                }
                            }

                            if (tilePositions[position.Value.x, position.Value.y + 1])
                            {
                                if (tilePositions[position.Value.x, position.Value.y + 1].tag == "corridor")
                                {
                                    instance = Instantiate(corridorWallObj, new Vector3(i, 1f, j), Quaternion.identity) as GameObject;
                                    instance.transform.SetParent(transform);
                                    continue;
                                }
                            }
                            instance = Instantiate(wallObj, new Vector3(i, 1f, j), Quaternion.identity) as GameObject;
                            instance.transform.SetParent(transform);
                        }
                    }
                }
            }
        }
        else
        {
            CreateWalls(subDungeon.left);
            CreateWalls(subDungeon.right);
        }
    }

    public void CreateRooms(SubFloor subDungeon)
    {
        if(subDungeon == null)
        {
            return;
        }

        if (subDungeon.AmALeaf())
        {
            for(int i = (int)subDungeon.room.x; i < subDungeon.room.xMax; i++)
            {
                for(int j = (int)subDungeon.room.y; j < subDungeon.room.yMax; j++)
                {
                    GameObject instance = Instantiate(floorObj, new Vector3(i, 0.5f, j), Quaternion.Euler(0,Random.Range(0,3)*90,0)) as GameObject;
                    instance.transform.SetParent(transform);
                    tilePositions[i, j] = instance;

                    if (!(i == (int)subDungeon.room.x || i == (int)subDungeon.room.xMax - 1 || j == (int)subDungeon.room.y || j == (int)subDungeon.room.yMax - 1))
                    {
                        subDungeon.floors.Add(instance);
                    }
                }
            }
        }
        else
        {
            CreateRooms(subDungeon.left);
            CreateRooms(subDungeon.right);
        }
    }

    public void CreateCorridors(SubFloor subDungeon)
    {
        if(subDungeon == null)
        {
            return;
        }

        CreateCorridors(subDungeon.left);
        CreateCorridors(subDungeon.right);
        foreach (Rect corridor in subDungeon.corridors)
        {
            for(int i = (int)corridor.x; i < corridor.xMax; i++)
            {
                for(int j = (int) corridor.y; j < corridor.yMax; j++)
                {
                    if(tilePositions[i,j] == null)
                    {
                        GameObject instance = Instantiate(corridorObj, new Vector3(i, 0f, j), Quaternion.identity);
                        instance.transform.SetParent(transform);
                        tilePositions[i, j] = instance;
                    }
                }
            }
        }
    }

    public void AddDungeonsToListReadyForDetails(SubFloor subDungeon)
    {
        if (subDungeon == null)
        {
            return;
        }

        if (subDungeon.AmALeaf())
        {
            subDungeons.Add(subDungeon);
        }
        else
        {
            AddDungeonsToListReadyForDetails(subDungeon.left);
            AddDungeonsToListReadyForDetails(subDungeon.right);
        }
    }

    public void Partition(SubFloor subDungeon)
    {
        if (subDungeon.AmALeaf())
        {
            if(subDungeon.rect.width > maxRoomSize || subDungeon.rect.height > maxRoomSize || Random.Range(0.0f, 1.0f) > 0.25)
            {
                if(subDungeon.CreateSubFloors(minRoomSize, maxRoomSize))
                {
                    Partition(subDungeon.left);
                    Partition(subDungeon.right);
                }
            }
        }
    }

    public List<SubFloor> GetSubDungeons() { return subDungeons; }

    public Vector2Int? GetPositionInArray(GameObject obj)
    {
        for(int i = 0; i < columns; i++)
        {
            for(int j = 0; j < rows; j++)
            {
                if(tilePositions[i,j] == obj)
                {
                    return new Vector2Int(i,j);
                }
            }
        }

        return null;
    }

    public struct Vector2Int
    {
        public int x;
        public int y;

        public Vector2Int(int _x, int _y)
        {
            x = _x;
            y = _y;
        }
    }
}

public class SubFloor
{
    public List<GameObject> floors = new List<GameObject>();
    public int enemiesInDungeon = 0;
    public List<Rect> corridors = new List<Rect>();
    public SubFloor left, right;
    public Rect rect;
    public Rect room = new Rect(-1, -1, 0, 0);
    public int debugID;
    private static int debugCounter = 0;
    public SubFloor(Rect rect)
    {
        this.rect = rect;
        debugID = debugCounter;
        debugCounter++;
    }
    public bool AmALeaf()
    {
        return left == null && right == null;
    }

    public bool CreateSubFloors(int minRoomSize, int maxRoomSize)
    {
        if (!AmALeaf())
        {
            return false;
        }

        bool horizontalLength;
        if (rect.width / rect.height >= 1.25f)
        {
            horizontalLength = false;
        }
        else if (rect.height / rect.width >= 1.25f)
        {
            horizontalLength = true;
        }
        else
        {
            horizontalLength = Random.Range(0.0f, 1.0f) > 0.5;
        }

        if (Mathf.Min(rect.height, rect.width) / 2 < minRoomSize)
        {
            return false;
        }

        if (horizontalLength)
        {
            int splitPoint = Random.Range(minRoomSize, (int)(rect.width - minRoomSize));

            left = new SubFloor(new Rect(rect.x, rect.y, rect.width, splitPoint));
            right = new SubFloor(new Rect(rect.x, rect.y + splitPoint, rect.width, rect.height - splitPoint));
        }

        else
        {
            int splitPoint = Random.Range(minRoomSize, (int)(rect.height - minRoomSize));
            left = new SubFloor(new Rect(rect.x, rect.y, splitPoint, rect.height));
            right = new SubFloor(new Rect(rect.x + splitPoint, rect.y, rect.width - splitPoint, rect.height));
        }

        return true;
    }

    public void CreateRoom()
    {
        if (left != null)
        {
            left.CreateRoom();
        }
        if (right != null)
        {
            right.CreateRoom();
        }
        if (left != null && right != null)
        {
            CreateCorridorBetween(left, right);
        }
        if (AmALeaf())
        {
            int roomWidth = (int)Random.Range(rect.width / 2, rect.width - 2);
            int roomHeight = (int)Random.Range(rect.height / 2, rect.height - 2);
            int roomX = (int)Random.Range(1, rect.width - roomWidth - 1);
            int roomY = (int)Random.Range(1, rect.height - roomHeight - 1);

            room = new Rect(rect.x + roomX, rect.y + roomY, roomWidth, roomHeight);
        }
    }

    public Rect GetRoom()
    {
        if (AmALeaf())
        {
            return room;
        }
        if (left != null)
        {
            Rect lroom = left.GetRoom();
            if (lroom.x != -1)
            {
                return lroom;
            }
        }
        if (right != null)
        {
            Rect rroom = right.GetRoom();
            if (rroom.x != -1)
            {
                return rroom;
            }
        }

        return new Rect(-1, -1, 0, 0);
    }

    public void CreateCorridorBetween(SubFloor left, SubFloor right)
    {
        Rect lroom = left.GetRoom();
        Rect rroom = right.GetRoom();

        int boundary = 2;
        Vector2 lpoint = new Vector2((int)Random.Range(lroom.x + boundary, lroom.xMax - boundary), (int)Random.Range(lroom.y + boundary, lroom.yMax - boundary));
        Vector2 rpoint = new Vector2((int)Random.Range(rroom.x + boundary, rroom.xMax - boundary), (int)Random.Range(rroom.y + boundary, rroom.yMax - boundary));

        if (lpoint.x > rpoint.x)
        {
            Vector2 temp = lpoint;
            lpoint = rpoint;
            rpoint = temp;
        }

        int w = (int)(lpoint.x - rpoint.x);
        int h = (int)(lpoint.y - rpoint.y);

        if (w != 0)
        {
            if (Random.Range(0, 1) > 2)
            {
                corridors.Add(new Rect(rpoint.x, lpoint.y, 2, Mathf.Abs(h)));
                if (h < 0)
                {
                    corridors.Add(new Rect(rpoint.x, lpoint.y, 2, Mathf.Abs(h)));
                }
                else
                {
                    corridors.Add(new Rect(rpoint.x, lpoint.y, 2, -Mathf.Abs(h)));
                }
            }
            else
            {
                if (h < 0)
                {
                    corridors.Add(new Rect(lpoint.x, lpoint.y, 2, Mathf.Abs(h)));
                }
                else
                {
                    corridors.Add(new Rect(lpoint.x, rpoint.y, 2, Mathf.Abs(h)));
                }

                corridors.Add(new Rect(lpoint.x, rpoint.y, Mathf.Abs(w) + 1, 2));
            }
        }
        else
        {
            if (h < 0)
            {
                corridors.Add(new Rect((int)lpoint.x, (int)lpoint.y, 2, Mathf.Abs(h)));
            }
            else
            {
                corridors.Add(new Rect((int)rpoint.x, (int)rpoint.y, 2, Mathf.Abs(h)));
            }
        }
    }

    public GameObject GetRandomValidPointInRoom(bool remove)
    {
        int counter = 0;
        GameObject floor = null;
        int index = 0;
        do
        {
            index = Random.Range(0, floors.Count - 1);
            floor = floors[index];
            counter++;
        } while (floor == null && counter < 100);

        if (remove)
        {
            if (floor != null)
            {
                RemoveFromList(floor);
            }
        }

        return floor;
    }

    public List<GameObject> GetAreaInRoom(int width, int height, ref bool success)
    {
        if((height > Get2DHeight()-1) || (width > Get2DWidth()-1) || height <= 0 || width <= 0)
        {
            return null;
        }

        List<GameObject> ret = new List<GameObject>();

        bool retry = false;
        int counter = 0;

        do
        {
            retry = false;
            ret = new List<GameObject>();
            int startingY = Random.Range(1, Get2DHeight() - (height + 1));
            int startingX = Random.Range(1, Get2DWidth() - (width + 1));

            for (int i = startingX; i < startingX + width; i++)
            {
                for (int j = startingY; j < startingY + height; j++)
                {
                    int index = (i * Get2DHeight()) + j;
                    ret.Add(floors[index]);
                    counter++;

                    if(floors[index] == null)
                    {
                        retry = true;
                    }
                }
            }
        } while (retry && counter < 10000);

        success = !retry;

        return ret;
    }

    public GameObject[,] Get2DFloors()
    {
        int height = Get2DHeight();
        int width = Get2DWidth();
        GameObject[,] Floors = new GameObject[height, width];

        int x = 0;
        for(int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                Floors[i, j] = floors[x];
                x++;
            }
        }

        return Floors;
    }

    public int Get2DHeight()
    {
        return (int)room.height - 2;
    }

    public int Get2DWidth()
    {
        return (int)room.width - 2;
    }

    public void RemoveFromList(int index)
    {
        if (index > -1 && index < floors.Count - 1)
        {
            if (floors[index] != null)
            {
                floors[index] = null;
            }
        }
    }

    public void RemoveFromList(GameObject obj)
    {
        int index = floors.IndexOf(obj);
        if (index > -1)
        {
            RemoveFromList(index);
        }
    }
}