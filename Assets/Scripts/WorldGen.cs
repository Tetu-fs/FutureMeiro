using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldGen : MonoBehaviour
{
    public int XLIMIT = 29;
    public int YLIMIT = 15;

    private int harfXLIM, quotaYLIM;

    public GameObject debugObj;

    public GameObject wall;
    public GameObject[] walls;
    public bool isMakingMaze, searchX, invertMath, cantX, cantY, makeWayX, makeWayY = false;
    private int rand;

    private int wallArrayLength;
    private int searchWaysLength;

    private float makingTime;

    private Vector3 startPos;
    private Vector3 goalPos;
    private Vector3 nowPos;
    private Vector3 nextPos;
    private Vector3 checkPosX, checkPosY;
    private Vector3 cantPos;

    private MeshRenderer wallMesh;

    public List<GameObject> searchWays = new List<GameObject>();

    public List<GameObject> goodWays = new List<GameObject>();
    public List<GameObject> removedWays = new List<GameObject>();

    public List<Vector3> badWays = new List<Vector3>();

    // Use this for initialization
    void Start()
    {
        harfXLIM = XLIMIT / 2;
        quotaYLIM = YLIMIT / 4;

        for (int x = 0; x < XLIMIT; x++)
        {
            for (int y = 0; y < YLIMIT; y++)
            {
                Instantiate<GameObject>(wall);
                wall.transform.position = new Vector2(x - harfXLIM, y - quotaYLIM);

            }
        }
        int startX = Random.Range(1, XLIMIT - 1);
        int startY = Random.Range(YLIMIT / 2, YLIMIT - 1);

        startPos = new Vector3(startX - harfXLIM, startY - quotaYLIM, 0);
        goalPos = new Vector3(0, -quotaYLIM, 0);

        walls = GameObject.FindGameObjectsWithTag("wall");
        wallArrayLength = walls.Length;


        for (int i = 0; i < wallArrayLength; i++)
        {
            walls[i].transform.SetParent(transform);
            if (walls[i].transform.position.x % 2 == 0 &&
                walls[i].transform.position.y % 2 == 0)
            {
                searchWays.Add(walls[i]);
            }
            if (walls[i].transform.position == goalPos ||
                walls[i].transform.position == new Vector3(goalPos.x, goalPos.y + 1, goalPos.z) ||
                walls[i].transform.position == new Vector3(goalPos.x, goalPos.y + 2, goalPos.z))
            {
                walls[i].GetComponent<MeshRenderer>().enabled = false; ;
            }
        }
        searchWaysLength = searchWays.Count;
        nowPos = new Vector3(goalPos.x, goalPos.y + 2, goalPos.z);

        isMakingMaze = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (isMakingMaze)
        {
            makingTime = Time.time;

            int randamSwitch = Random.Range(0, 2);


            switch (randamSwitch)
            {
                case 0:
                    searchX = true;
                    break;

                case 1:
                    searchX = false;
                    break;
            }
            rand = Random.Range(0, 2);

            if (rand == 0)
            {
                rand -= 1;
            }
            if (!cantX || !cantY)
            {

                if (searchX)
                {
                    if (!invertMath)
                    {
                        checkPosX = new Vector3(nowPos.x + rand * 2, nowPos.y, nowPos.z);
                        nextPos = new Vector3(checkPosX.x - rand, checkPosX.y, checkPosX.z);
                    }
                    else
                    {
                        checkPosX = new Vector3(nowPos.x - rand * 2, nowPos.y, nowPos.z);
                        nextPos = new Vector3(checkPosX.x + rand, checkPosX.y, checkPosX.z);
                    }
                    for (int j = 0; j < searchWaysLength; j++)
                    {
                        if (searchWays[j].transform.position == checkPosX &&
                            searchWays[j].transform.position.x != -harfXLIM &&
                            searchWays[j].transform.position.y != -quotaYLIM &&
                            searchWays[j].transform.position.x != harfXLIM &&
                            searchWays[j].transform.position.y != YLIMIT - 1 - quotaYLIM &&
                            searchWays[j].GetComponent<MeshRenderer>().enabled == true)
                        {
                            makeWayX = true;
                            searchWays[j].SendMessage("MeshDenable");

                            break;
                        }
                        makeWayX = false;
                    }
                    if (makeWayX)
                    {
                        Debug.Log("checkX");
                        for (int i = 0; i < wallArrayLength; i++)
                        {
                            if (walls[i].transform.position == nextPos &&
                                walls[i].GetComponent<MeshRenderer>().enabled == true)
                            {
                                debugObj.transform.position = checkPosX;
                                walls[i].SendMessage("MeshDenable");
                                nowPos = checkPosX;

                                break;
                            }
                        }
                    }
                    else if (!cantX)
                    {
                        cantX = true;
                    }
                    else
                    {
                        if (!invertMath)
                        {
                            invertMath = true;
                            cantX = false;
                        }
                        else
                        {
                            invertMath = false;
                            searchX = false;

                        }

                    }
                }

                else
                {
                    if (!invertMath)
                    {
                        checkPosY = new Vector3(nowPos.x, nowPos.y + rand * 2, nowPos.z);
                        nextPos = new Vector3(checkPosY.x, checkPosY.y - rand, checkPosY.z);
                    }
                    else
                    {
                        checkPosY = new Vector3(nowPos.x, nowPos.y - rand * 2, nowPos.z);
                        nextPos = new Vector3(checkPosY.x, checkPosY.y + rand, checkPosY.z);
                    }
                    for (int j = 0; j < searchWaysLength; j++)
                    {
                        if (searchWays[j].transform.position == checkPosY &&
                            searchWays[j].transform.position.x != -harfXLIM &&
                            searchWays[j].transform.position.y != -quotaYLIM &&
                            searchWays[j].transform.position.x != harfXLIM &&
                            searchWays[j].transform.position.y != YLIMIT - 1 - quotaYLIM &&
                            searchWays[j].GetComponent<MeshRenderer>().enabled == true)
                        {
                            makeWayY = true;
                            searchWays[j].SendMessage("MeshDenable");

                            break;
                        }
                        makeWayY = false;

                    }
                    if (makeWayY)
                    {
                        Debug.Log("checkY");

                        for (int i = 0; i < wallArrayLength; i++)
                        {
                            if (walls[i].transform.position == nextPos &&
                                walls[i].GetComponent<MeshRenderer>().enabled == true)
                            {
                                debugObj.transform.position = checkPosY;
                                walls[i].SendMessage("MeshDenable");
                                nowPos = checkPosY;

                                break;
                            }
                        }
                    }
                    else if (!cantY)
                    {
                        cantY = true;
                    }
                    else
                    {
                        if (!invertMath)
                        {
                            invertMath = true;
                            cantY = false;
                        }
                        else
                        {
                            invertMath = false;
                            searchX = true;

                        }

                    }
                }
            }
            else
            {
                cantPos = nowPos;
                if (!badWays.Contains(cantPos))
                {
                    badWays.Add(cantPos);
                }
                for (int i = 0; i < searchWaysLength; i++)
                {
                    if (searchWays[i].GetComponent<MeshRenderer>().enabled == false)
                    {
                        if (!badWays.Contains(searchWays[i].transform.position) && !removedWays.Contains(searchWays[i]))
                        {
                            if (!goodWays.Contains(searchWays[i]))
                            {
                                goodWays.Add(searchWays[i]);
                            }
                        }
                        else
                        {
                            goodWays.Remove(searchWays[i]);
                            if (!removedWays.Contains(searchWays[i]))
                            {
                                removedWays.Add(searchWays[i]);
                            }
                        }

                    }
                }
                int randChoice = Random.Range(0, goodWays.Count);

                if (goodWays.Count > 0)
                {
                    debugObj.transform.position = goodWays[randChoice].transform.position;

                    nowPos = goodWays[randChoice].transform.position;
                    //goodWays.Clear();
                    cantX = false;
                    cantY = false;
                }
                else
                {

                    cantX = false;
                    cantY = false;
                    Debug.Log(makingTime);

                    isMakingMaze = false;

                }


            }
        }
    }
}