using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldGen : MonoBehaviour
{
    public int XLIMIT = 29;
    public int YLIMIT = 15;

    private int harfXLIM,quotaYLIM;

    public GameObject debugObj;

    public GameObject wall;
    public GameObject[] walls;
    public bool isMakingMaze, searchX, invertMath, cantX, cantY = false;
    private int rand;

    private float makingTime;

    private Vector3 startPos;
    private Vector3 goalPos;
    private Vector3 nowPos;
    private Vector3 nextPos;
    private Vector3 checkPosX, checkPosY;
    private Vector3 cantPos;

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
        walls = GameObject.FindGameObjectsWithTag("wall");

        int startX = Random.Range(1, XLIMIT - 1);
        int startY = Random.Range(YLIMIT / 2, YLIMIT - 1);

        startPos = new Vector3(startX - harfXLIM, startY - quotaYLIM, 0);
        goalPos = new Vector3(0, -quotaYLIM, 0);

        for (int i = 0; i < walls.Length; i++)
        {

            walls[i].transform.SetParent(transform);
            if (walls[i].transform.position == goalPos ||
                walls[i].transform.position == new Vector3(goalPos.x, goalPos.y + 1, goalPos.z) ||
                walls[i].transform.position == new Vector3(goalPos.x, goalPos.y + 2, goalPos.z))
            {
                walls[i].GetComponent<MeshRenderer>().enabled = false;
            }
        }

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

            walls = GameObject.FindGameObjectsWithTag("wall");
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
                    for (int j = 0; j < walls.Length; j++)
                    {
                     
                        if (walls[j].transform.position == checkPosX &&
                            walls[j].transform.position.x != -harfXLIM &&
                            walls[j].transform.position.y != -quotaYLIM &&
                            walls[j].transform.position.x != harfXLIM &&
                            walls[j].transform.position.y != YLIMIT - 1 - quotaYLIM &&
                            walls[j].GetComponent<MeshRenderer>().enabled == true)

                        {
                            for (int i = 0; i < walls.Length; i++)
                            {
                                if (walls[i].transform.position == nextPos &&
                                    walls[i].GetComponent<MeshRenderer>().enabled == true)
                                {
                                    debugObj.transform.position = checkPosX;
                                    walls[i].GetComponent<MeshRenderer>().enabled = false;
                                    nowPos = checkPosX;

                                    cantX = false;
                                    break;
                                }
                                cantX = true;
                            }
                            walls[j].GetComponent<MeshRenderer>().enabled = false;

                            break;
                        }
                        else if (!cantX)
                        {
                            cantX = true;
                        }
                    }
                    if (cantX)
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
                        Debug.Log("checkY");

                        checkPosY = new Vector3(nowPos.x, nowPos.y - rand * 2, nowPos.z);
                        nextPos = new Vector3(checkPosY.x, checkPosY.y + rand, checkPosY.z);
                    }
                    for (int j = 0; j < walls.Length; j++)
                    {
                       
                        if (walls[j].transform.position == checkPosY &&
                            walls[j].transform.position.x != -harfXLIM &&
                            walls[j].transform.position.y != -quotaYLIM &&
                            walls[j].transform.position.x != harfXLIM &&
                            walls[j].transform.position.y != YLIMIT - 1 - quotaYLIM &&
                            walls[j].GetComponent<MeshRenderer>().enabled == true)

                        {
                            for (int i = 0; i < walls.Length; i++)
                            {
                                if (walls[i].transform.position == nextPos &&
                                    walls[i].GetComponent<MeshRenderer>().enabled == true)
                                {
                                    debugObj.transform.position = checkPosY;
                                    walls[i].GetComponent<MeshRenderer>().enabled = false;
                                    nowPos = checkPosY;

                                    cantY = false;
                                    break;
                                }
                                cantY = true;
                            }
                            walls[j].GetComponent<MeshRenderer>().enabled = false;

                            break;
                        }
                        else if (!cantY)
                        {                            
                            cantY = true;
                        }
                    }
                    if (cantY)
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
                for (int i = 0; i < walls.Length; i++)
                {
                    if (walls[i].GetComponent<MeshRenderer>().enabled == false &&
                        walls[i].transform.position.x % 2 == 0 &&
                        walls[i].transform.position.y % 2 == 0)
                    {

                        if (!badWays.Contains(walls[i].transform.position) && !removedWays.Contains(walls[i]))
                        {
                            if (!goodWays.Contains(walls[i]))
                            {
                                goodWays.Add(walls[i]);
                            }
                        }
                        else
                        {
                            goodWays.Remove(walls[i]);
                            if (!removedWays.Contains(walls[i]))
                            {
                                removedWays.Add(walls[i]);
                                Debug.Log(makingTime);

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
        else
        {
        }
    }
}